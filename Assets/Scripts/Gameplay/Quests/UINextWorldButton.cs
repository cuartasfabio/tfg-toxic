using Backend.Localization;
using Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Quests
{
	public class UINextWorldButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private Image _image;
		[SerializeField] private Image _imageBg;
		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _tmpNext;
		[SerializeField] private string _nextTextId;

		private Color _highlightBg;

		private RectTransform _rect;
		private void Awake()
		{
			_highlightBg = new Color(0.9218402f,0.6839622f,1f,1f);
			_rect = GetComponent<RectTransform>();
			_tmpNext.text = StringBank.GetStringRaw(_nextTextId);
		}

		private void Start()
		{
			_button.onClick.AddListener(JumpToNextWorld);
		}

		private void JumpToNextWorld()
		{
			// run the close fog animation...
			
			// Debug.Log("Transitioning to next world!... (To Do)");

			GameSceneManager.Current.GoToNextWorld();
			
			Destroy(gameObject);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_imageBg.material.SetColor("_Tint", _highlightBg);
			_tmpNext.gameObject.SetActive(true);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, Vector3.one, new Vector3(1.2f, 1.2f, 1.2f), TfMath.EaseInQuad, 0.05f));
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			// _imageBg.gameObject.SetActive(false);
			_imageBg.material.SetColor("_Tint", Color.black);
			_tmpNext.gameObject.SetActive(false);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, TfMath.EaseInQuad, 0.05f));
		}
	}
}