using Backend.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Cards
{
	public class UICardPreview : MonoBehaviour
	{
		private UICardData _uiCardData;
        
        [SerializeField] private Image _baseImage;
        [SerializeField] private Image _ilustration;
        [SerializeField] private Image _cardDetail;
        [SerializeField] private TextMeshProUGUI _cardName;
        [SerializeField] private RectTransform _previewRectTransform;
        
        private Vector3 _tile;
        private bool _toDeck;

        public void Init(UICardData uiCardData, Sprite cardSpr, Vector3 tile)
        {
            _uiCardData = uiCardData;
            _baseImage.sprite = cardSpr;
            _ilustration.sprite = _uiCardData.CardSprite;
            _cardDetail.color = _uiCardData.DetailColor;
            _cardName.text = StringBank.GetStringRaw(_uiCardData.CardName);

            _tile = tile;
            _previewRectTransform.position = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tile);
        }

        public void Init(UICardData uiCardData, Sprite cardSpr)
        {
	        _uiCardData = uiCardData;
	        _baseImage.sprite = cardSpr;
	        _ilustration.sprite = _uiCardData.CardSprite;
	        _cardDetail.color = _uiCardData.DetailColor;
	        _cardName.text = StringBank.GetStringRaw(_uiCardData.CardName);
	        _toDeck = true;
        }
        
        private void Update()
        {
	        if(_toDeck) return;
	        
	        _previewRectTransform.position = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tile);
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
	}
}