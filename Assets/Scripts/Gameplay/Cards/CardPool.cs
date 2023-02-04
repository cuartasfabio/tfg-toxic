using System.Collections.Generic;
using Backend.Persistence;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.Cards
{
	public class CardPool : MonoBehaviour
	{
		[SerializeField] private UICardData[] _allCards;
		
		[SerializeField] private GameObject _uiCardPrefab;
		
		/// <summary>
		/// The sprite variations for the card base.
		/// </summary>
		[SerializeField] private Sprite[] _baseVariations;
		
		/// <summary>
		/// The preview shown when a card will be added to the player hand.
		/// </summary>
		[Space(10)]
		[SerializeField] private GameObject _uiCardPreviewPrefab;
		
		/// <summary>
		/// The preview shown when a card will be shuffled into the deck.
		/// </summary>
		[SerializeField] private GameObject _uiCardToDeckPreviewPrefab;
		
		[SerializeField] private Transform _previewPanel;

		[SerializeField] private Material _destroyCardMat;
		public Material DestroyCardMat => _destroyCardMat;

		private Dictionary<TileType, UICardData> _cardData;
		
		public void Init()
		{
			_cardData = new Dictionary<TileType, UICardData>();

			for (int i = 0; i < _allCards.Length; i++)
				_cardData.Add(_allCards[i].tileId, _allCards[i]);
		}

		public UICard GetCardOfType(TileType tileType)
		{
			UICard card =  Instantiate(_uiCardPrefab).GetComponent<UICard>(); 
			card.Init(_cardData[tileType], RandomBaseVariation());
			return card;
		}
		
		public UICardPreview GetCardPreview(TileType tileType, Vector3 origin)
		{
			UICardPreview preview = Instantiate(_uiCardPreviewPrefab, _previewPanel).GetComponent<UICardPreview>(); 
			preview.Init(_cardData[tileType],RandomBaseVariation(), origin);
			return preview;
		}
		
		public UICardToDeckPreview GetCardToDeckPreview(TileType tileType, Vector3 origin)
		{
			UICardToDeckPreview preview = Instantiate(_uiCardToDeckPreviewPrefab, _previewPanel).GetComponent<UICardToDeckPreview>(); 
			preview.Init(_cardData[tileType],RandomBaseVariation(), origin);
			return preview;
		}
		
		private Sprite RandomBaseVariation()
		{
			return _baseVariations[Random.Range(0,_baseVariations.Length)];
		}

		
		public UICardData GetCardDataOfType(TileType type)
		{
			if (!_cardData.ContainsKey(type))
			{
				Debug.Log("Missing ["+ type + "] uiCardData!");
				return _cardData[TileType.None];
			}
			return _cardData[type];
		}
		
		
	}
}