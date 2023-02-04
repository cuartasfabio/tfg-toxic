using System.Collections;
using System.Collections.Generic;
using Audio;
using Backend.Persistence;
using Gameplay.Tiles;
using UnityEngine;
using UnityEngine.UI;
using Utils;


namespace Gameplay.Cards
{
    public class UICardHand: MonoBehaviour, IPersistable
    {
        private const int _maxCardsInHand = 8;
        private const int _cardsGivenAtObjective = 3;
        
        [SerializeField] private RectTransform _cardPanel;
        [SerializeField] private RectTransform _cardSpawnPoint;
        
        [SerializeField] private GameObject _cardAnchorPrefab;
        
        private List<UICard> _cardsOnHand;
        
        public void Init()    
        {
            if (_cardsOnHand != null)
            {
                CleanHand();
            }
            else
            {
                _cardsOnHand = new List<UICard>();
            }
            DrawCards(4); // cards in hand at the start of the game
        }
        

        public void DrawCards(int cardNum = 0)
        {
            int cardsToDraw = cardNum == 0 ? _cardsGivenAtObjective : cardNum;
            
            int futureCardsInHand = _cardsOnHand.Count;
            for (int i = 0; i < cardsToDraw; i++)
            {
                if (futureCardsInHand < _maxCardsInHand)
                {
                    futureCardsInHand++;
                }
                else
                {
                    // when the hand is full, destroy card an the far left
                    RemoveCard(_cardsOnHand[0]);
                }
            }
            RandomCardsToHand(cardsToDraw);
        }

        public void CardToHand(TileType tileType, bool fromCheats = false)
        {
            UICard card = ObjectCache.Current.CardPool.GetCardOfType(tileType);
            if (fromCheats) card.EnableButton();
            StartCoroutine(PlaceCardOnCanvas(card));
        }

        private void RandomCardsToHand(int cards)
        {
            List<TileType> tileTypes = new List<TileType>();
            for (int i = 0; i < cards; i++)
            {
                tileTypes.Add(ObjectCache.Current.CardDeck.DrawRandomCardType());
            }

            StartCoroutine(PlaceCardsOnCanvas(tileTypes));
        }

        private IEnumerator PlaceCardsOnCanvas(List<TileType> tileTypes)
        {
            DisableCards();
            for (int i = 0; i < tileTypes.Count; i++)
            {
                UICard card = ObjectCache.Current.CardPool.GetCardOfType(tileTypes[i]);
                
                if (i == tileTypes.Count - 1)
                {
                    yield return StartCoroutine(PlaceCardOnCanvas(card));
                    EnableCards();
                }
                else
                {
                    StartCoroutine(PlaceCardOnCanvas(card));
                    yield return new WaitForSeconds(0.6f);
                }
            }
        }

        private IEnumerator PlaceCardOnCanvas(UICard card)  
        {
            AudioController.Get().PlaySfx(AudioId.SFX_ReceiveCard);
            _cardsOnHand.Add(card);

            RectTransform uiCard = card.GetComponent<RectTransform>();
            uiCard.SetParent(_cardSpawnPoint, false);
            card.Border.gameObject.SetActive(true);
            StartCoroutine(AnimationsController.ScaleUiElement(uiCard, new Vector3(0.6f,0.6f,0.6f), Vector3.one, TfMath.EaseInOutQuint, 0.35f));
            yield return StartCoroutine(AnimationsController.MoveUIElementY(uiCard,0,400f,TfMath.EaseInOutQuint, 0.35f));
            yield return new WaitForSeconds(0.25f);
            card.Border.gameObject.SetActive(false);
            
            // instanciar un cardAnchor en _cardPanel con Scale.x = 0
            RectTransform cardAnchor = Instantiate(_cardAnchorPrefab, _cardPanel).GetComponent<RectTransform>();
            
            // StartCoroutine() -- animar Scale.x de 0 a 1 para cardAnchor
            IEnumerator updateLayoutCoroutine = UpdateCardLayoutGroup();
            StartCoroutine(updateLayoutCoroutine);
            StartCoroutine(AnimationsController.ScaleUiElement(cardAnchor, Vector3.one, TfMath.EaseInOutQuint, 0.3f ));
            // StartCoroutine() -- mover uiCard hasta GlobalPosition de cardAnchor
            yield return StartCoroutine(AnimationsController.MoveUIElementToTransform(uiCard,cardAnchor, TfMath.EaseInOutQuint, 0.3f));
            
            // cuando acaben ambos...
            
            // emparentar uiCard en _cardPanel
            uiCard.SetParent(_cardPanel, false);
            // destruir carAnchor
            Destroy(cardAnchor.gameObject);
            
            StopCoroutine(updateLayoutCoroutine);
            
            // card.EnableButton();
        }

        private IEnumerator UpdateCardLayoutGroup()
        {
            while (true)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_cardPanel);
                yield return new WaitForSeconds(0.001f);
            }
        }
        
        public void ConsumeAndRemoveCard(UICard card)
        {
            StartCoroutine(ConsumeCardOnPlaceCoroutine(card));
        }

        private IEnumerator ConsumeCardOnPlaceCoroutine(UICard card)
        {
            yield return card.DestroyCardAnimation();
            RemoveCard(card);
            // EnableCards();
        }
        
        private void RemoveCard(UICard card)   
        {
            _cardsOnHand.Remove(card);
            card.DestroySelf();
        }

        public void EnableCards()   
        {
            foreach (UICard card in _cardsOnHand)
                card.EnableButton();
        }

        public void DisableCards() 
        {
            foreach (UICard card in _cardsOnHand)
                card.DisableButton();
        }

        public List<TileType> GetHandTypes()
        {
            HashSet<TileType> types = new HashSet<TileType>();

            for (int i = 0; i < _cardsOnHand.Count; i++)
                types.Add(_cardsOnHand[i].GetCardTileType());

            return new List<TileType>(types);
        }

        public void CleanHand()
        {
            if (_cardsOnHand == null) return;
            
            while (_cardsOnHand.Count > 0)
            {
                _cardsOnHand[0].DestroySelf();
                _cardsOnHand.Remove(_cardsOnHand[0]);
            }
        }
        
        // -------------------------------------------------------------------------------------------------------------

        public void Save(GameDataWriter writer)
        {
            writer.Write(_cardsOnHand.Count);
            for (int i = 0; i < _cardsOnHand.Count; i++)
            {
                writer.Write((int) _cardsOnHand[i].UICardData.tileId);
            }
        }

        public void Load(GameDataReader reader)
        {
            _cardsOnHand = new List<UICard>();
            
            int cardNum = reader.ReadInt();

            for (int i = 0; i < cardNum; i++)
            {
                LoadCard((TileType) reader.ReadInt());
            }
        }
        
        private void LoadCard(TileType tileType)
        {
            UICard card = ObjectCache.Current.CardPool.GetCardOfType(tileType);
            card.EnableButton();
            _cardsOnHand.Add(card);
            RectTransform uiCard = card.GetComponent<RectTransform>();
            uiCard.SetParent(_cardPanel, false);
        }
    }
}