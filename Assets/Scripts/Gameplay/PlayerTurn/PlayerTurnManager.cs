using Gameplay.Cards;
using Gameplay.PlayerControllers;
using Gameplay.Tiles;
using UI.InGames.TileBorders;
using UnityEngine;

namespace Gameplay.PlayerTurn
{
    public class PlayerTurnManager : MonoBehaviour
    {
        public UICard SelectedCard { get; private set; }
        public PlayerTurnStateIdle PlayerTurnStateIdle { get; private set; }
        public PlayerTurnStatePlacing PlayerTurnStatePlacing { get; private set; }
        public UITileBorder HoveredTileBorder { get; private set; }
        
        private IPlayerTurnState _currentState;
        private TileBehaviour _behaviourPreview;
        
        public void Init()
        {
            if (HoveredTileBorder != null) Destroy(HoveredTileBorder.gameObject);
            HoveredTileBorder = ObjectCache.Current.TileBorderPool.GetTileBorderObject();
            
            PlayerTurnStateIdle = new PlayerTurnStateIdle(this);
            PlayerTurnStatePlacing = new PlayerTurnStatePlacing(this);
            _currentState = PlayerTurnStateIdle;
            _currentState.Enter(); 
        }
        
        private void Update()
        {
            if(_currentState != null) _currentState.UpdateState();
        }
        
        public void ChangeState(IPlayerTurnState newState)
        {
            if (newState.Equals(_currentState)) return;
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void PickUpUICard(UICard card)
        {
            SelectedCard = card;

            ObjectCache.Current.HelpManager.UpdateHelpPanel(card.UICardData);

            ChangeState(PlayerTurnStatePlacing);
        }

        public void CancelPlacing()
        {
            if (_currentState != PlayerTurnStateIdle)
            {
                if (SelectedCard!= null) SelectedCard.PutBackCard();
                SelectedCard = null;
                
                _behaviourPreview.DeleteSelf();
                _behaviourPreview = null;

                ObjectCache.Current.UiCardHand.EnableCards();
                ChangeState(PlayerTurnStateIdle);
            }
        }
        
        public void EndPlacing()
        {
            if (_currentState != PlayerTurnStateIdle)
            {
                // ObjectCache.Current.UiCardHand.ConsumeAndRemoveCard(SelectedCard); now is in command
                SelectedCard = null;
                ObjectCache.Current.UiCardHand.EnableCards();
                _behaviourPreview = null;
                
                ChangeState(PlayerTurnStateIdle);
            }
        }

        public TileBehaviour GetBehaviourPreview()
        {
            if (_behaviourPreview == null)
                _behaviourPreview = SelectedCard.GetBehaviourPreview();
            return _behaviourPreview;
        }
        
    }
}