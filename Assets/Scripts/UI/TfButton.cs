using Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace UI
{
	public class TfButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
	{
		private RectTransform _rect;
		private void Awake()
		{
			_rect = GetComponent<RectTransform>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			// AudioController.Get().PlayAudio(AudioType.SFX_HoverButton);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			AudioController.Get().PlaySfx(AudioId.SFX_Click1);
		}

		public void PopUp()
		{
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, Vector3.zero, Vector3.one, TfMath.EaseOutElastic, 0.3f));
		}
	}
}