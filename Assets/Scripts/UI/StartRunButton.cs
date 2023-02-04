using Audio;
using Backend.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
	public class StartRunButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
	{
		private RectTransform _rect;
		[SerializeField] private Image _imageBg;
		[SerializeField] private TMP_Text _tmpNext;
		[SerializeField] private string _nextTextId;

		private Color _highlightBg;
		private void Awake()
		{
			_highlightBg = new Color(0.9218402f,0.6839622f,1f,1f);
			_rect = GetComponent<RectTransform>();
			_tmpNext.text = StringBank.GetStringRaw(_nextTextId);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_imageBg.material.SetColor("_Tint", _highlightBg);
			_tmpNext.gameObject.SetActive(true);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, Vector3.one, new Vector3(1.2f, 1.2f, 1.2f), TfMath.EaseInQuad, 0.05f));
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_imageBg.material.SetColor("_Tint", Color.black);
			_tmpNext.gameObject.SetActive(false);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, TfMath.EaseInQuad, 0.05f));
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