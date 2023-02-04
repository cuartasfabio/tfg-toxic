using System.Collections.Generic;
using Backend.Localization;
using TMPro;
using UnityEngine;

namespace UI.Menus.CardCollections
{
	public class CardCollection : MonoBehaviour
	{
		[SerializeField] private Sprite[] _baseVariations;
		[Space]
		[SerializeField] private Sprite[] _categoryBorders;
		[Space]
		[SerializeField] private CardDetails _details;
		[Space]
		[SerializeField] private RectTransform _defaultsPanel;
		[SerializeField] private RectTransform _world1Panel;
		[SerializeField] private RectTransform _world2Panel;
		[SerializeField] private RectTransform _world3Panel;
		[Space] 
		[SerializeField] private CardListData _defaultInfo;
		[SerializeField] private CardListData _world1Info;
		[SerializeField] private CardListData _world2Info;
		[SerializeField] private CardListData _world3Info;
		[Space] 
		[SerializeField] private TMP_Text _baseCardsTMP;
		[SerializeField] private TMP_Text _world1TMP;
		[SerializeField] private TMP_Text _world2TMP;
		[SerializeField] private TMP_Text _world3TMP;
		[Space] 
		[SerializeField] private GameObject _cardElementPrefab;
		[Space] 
		[SerializeField] private Material _baseCardMat;
		[SerializeField] private Material _world1CardMat;
		[SerializeField] private Material _world2CardMat;
		[SerializeField] private Material _world3CardMat;
		
		private List<CardElement> _defaultCards;
		private List<CardElement> _world1Cards;
		private List<CardElement> _world2Cards;
		private List<CardElement> _world3Cards;

		public void FillCollection()
		{
			_defaultCards = new List<CardElement>();
			_world1Cards = new List<CardElement>();
			_world2Cards = new List<CardElement>();
			_world3Cards = new List<CardElement>();

			_baseCardsTMP.text = StringBank.GetStringRaw("CARD_MENU_BASE");
			_world1TMP.text = StringBank.GetStringRaw("CARD_MENU_WORLD1");
			_world2TMP.text = StringBank.GetStringRaw("CARD_MENU_WORLD2");
			_world3TMP.text = StringBank.GetStringRaw("CARD_MENU_WORLD3");
			
			
			for (int i = 0; i < _defaultInfo.Cards.Length; i++)
			{
				CardElement cardElm = Instantiate(_cardElementPrefab, _defaultsPanel).GetComponent<CardElement>();
				cardElm.Init(this, _defaultInfo.Cards[i], _baseCardMat);
				_defaultCards.Add(cardElm);
			}
			
			for (int i = 0; i < _world1Info.Cards.Length; i++)
			{
				CardElement cardElm = Instantiate(_cardElementPrefab, _world1Panel).GetComponent<CardElement>();
				cardElm.Init(this, _world1Info.Cards[i], _world1CardMat);
				_world1Cards.Add(cardElm);
			}
			
			for (int i = 0; i < _world2Info.Cards.Length; i++)
			{
				CardElement cardElm = Instantiate(_cardElementPrefab, _world2Panel).GetComponent<CardElement>();
				cardElm.Init(this, _world2Info.Cards[i], _world2CardMat);
				_world2Cards.Add(cardElm);
			}
			
			for (int i = 0; i < _world3Info.Cards.Length; i++)
			{
				CardElement cardElm = Instantiate(_cardElementPrefab, _world3Panel).GetComponent<CardElement>();
				cardElm.Init(this, _world3Info.Cards[i], _world3CardMat);
				_world3Cards.Add(cardElm);
			}
		}

		public Sprite GetRandomBaseSprite()
		{
			return _baseVariations[Random.Range(0,_baseVariations.Length)]; 
		}

		public Sprite GetBorderForCategory(int catIndex)
		{
			return _categoryBorders[catIndex];
		}

		public CardDetails GetDetailsObject()
		{
			return _details;
		}
	}
}