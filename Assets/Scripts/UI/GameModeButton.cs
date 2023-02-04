using Backend.Localization;
using Gameplay.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
	public class GameModeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
	{
		[SerializeField] private GameMode _gameMode;
		
		[SerializeField] private Image _bg;
		[SerializeField] private Button _button;

		[SerializeField] private StartRunButton _startRunBtn;
		[SerializeField] private Button _playBtn;
		[SerializeField] private TMP_Text _modeName;
		[SerializeField] private TMP_Text _modeDesc;

		[Space] 
		[SerializeField] private string _modeName_LocId;
		[SerializeField] private string _modeDesc_LocId;

		[SerializeField] private float _bgHueShift;

		// [SerializeField] private Color _borderColor;

		private RectTransform _rect;

		private bool _selected;

		private void Awake()
		{
			_rect = GetComponent<RectTransform>();
		}

		public void Init()
		{
			_button.onClick.AddListener(ClickOnMode);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (_selected) return;
			
			_bg.gameObject.SetActive(true);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, Vector3.one, new Vector3(1.2f, 1.2f, 1.2f), TfMath.EaseInQuad, 0.05f));
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (_selected) return;
			
			_bg.gameObject.SetActive(false);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, TfMath.EaseInQuad, 0.05f));
		}

		private void ClickOnMode()
		{
			_selected = true;
			
			// todo RunConfigMenu.newRun = new RunManager(_gameModeConfig)
			// _playBtn.onClick.AddListener(() => { GameManager.Get().RunManager = new RunManager(_gameModeConfig); });
			_playBtn.onClick.AddListener(() => { GameManager.Get().CreateRun(_gameMode); });

			_startRunBtn.gameObject.SetActive(true);
			_startRunBtn.PopUp();
			
			_bg.gameObject.SetActive(true);

			
			_modeName.text = StringBank.GetStringRaw(_modeName_LocId);
			_modeDesc.text = StringBank.GetStringRaw(_modeDesc_LocId);
		}

		// public void OnDeselect(BaseEventData eventData)
		// {
		// 	_selected = false;
		// 	
		// 	_playBtn.gameObject.SetActive(false);
		// 	
		// 	// _bg.gameObject.SetActive(false);
		// 	StartCoroutine(AnimationsController.ScaleUiElement(_rect, new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, TfMath.EaseInQuad, 0.1f));
		// }
		

		public void OnDeselect(BaseEventData eventData)
		{
			_selected = false;
			// Debug.Log("Deselected" + _modeName_LocId);
			_bg.gameObject.SetActive(false);
			StartCoroutine(AnimationsController.ScaleUiElement(_rect, new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, TfMath.EaseInQuad, 0.05f));
		}
	}
}