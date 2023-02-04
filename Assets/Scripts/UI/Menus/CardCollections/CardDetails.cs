using Backend.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus.CardCollections
{
	public class CardDetails : MonoBehaviour
	{
		[SerializeField] private Image _cardBase;
		[SerializeField] private Image _cardImg;
		[SerializeField] private Image _cardBorder;
		[SerializeField] private TMP_Text _descriptionText;
		[SerializeField] private TMP_Text _carNameText;

		// Initially, show de details of the first card
		public void UpdateCardDetails(CardCollection cardCollection, CardListData.CardConfig cardConfig)
		{
			_cardBase.material = cardConfig.cardData.CardBaseMat;
			_cardImg.sprite = cardConfig.cardData.CardSprite;
			_cardBorder.sprite = cardCollection.GetBorderForCategory((int) cardConfig.Category);
			_cardBorder.color = cardConfig.cardData.DetailColor;
			_carNameText.text = StringBank.GetStringRaw(cardConfig.cardData.CardName);
			_descriptionText.text = StringBank.GetStringRaw(cardConfig.cardData.Synergies);
		}
	}
}