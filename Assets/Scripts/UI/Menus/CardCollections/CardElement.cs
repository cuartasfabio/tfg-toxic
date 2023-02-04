using Backend.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus.CardCollections
{
	
	
	public class CardElement : MonoBehaviour
	{
		[SerializeField] private Button _cardBtn;
		[SerializeField] private Image _base;
		[SerializeField] private Image _illustration;
		[SerializeField] private Image _border;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private Image _blocked;

		private CardStatus _currentStatus;

		//TODO Unlock progress ...

		private CardListData.CardConfig _config;
		private CardCollection _cardCollection;


		public void Init(CardCollection cardCollection, CardListData.CardConfig config, Material cardBaseMat)
		{
			_cardCollection = cardCollection;
			_config = config;
			
			// configs itself
			_base.sprite = cardCollection.GetRandomBaseSprite();
			// _base.material = config.CardInfo.CardBaseMat;
			_base.material = cardBaseMat;
			_illustration.sprite = config.cardData.CardSprite;
			_border.sprite = cardCollection.GetBorderForCategory((int) config.Category);
			_border.color = config.cardData.DetailColor;
			_name.text = StringBank.GetStringRaw(config.cardData.CardName);

			// todo get the status from the player saved data
			SetUnlockStatus(_config.DefaultStatus);

		}

		private void Start()
		{
			_cardBtn.onClick.AddListener(ShowDetails);
		}

		private void ShowDetails()
		{
			// TODO add Description to _config
			_cardCollection.GetDetailsObject().UpdateCardDetails(_cardCollection, _config);
		}

		private void SetUnlockStatus(CardStatus currentStatus)
		{
			_currentStatus = currentStatus;

			if (_currentStatus == CardStatus.BLOCKED)
			{
				_cardBtn.interactable = false;
				_blocked.color = new Color(1, 1, 1, 0.5f);
			}
				
		}
	}

	
}