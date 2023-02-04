using Backend.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Cards
{
	public class UICardToDeckPreview : MonoBehaviour
	{
		[SerializeField] private UICardPreview _cardPreview;
		[SerializeField] private RectTransform _previewRectTransform;
		private Vector3 _tile;

		public void Init(UICardData uiCardData, Sprite cardSpr, Vector3 tile)
		{
			_cardPreview.Init(uiCardData, cardSpr);

			_tile = tile;
			_previewRectTransform.position = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tile);
		}
        
		private void Update()
		{
			_previewRectTransform.position = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tile);
		}
		
		public void DestroySelf()
		{
			Destroy(gameObject);
		}
		
	}
}