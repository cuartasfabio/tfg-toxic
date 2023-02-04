using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.InGames.DescriptionHelp
{
	public class HelpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private Image _image;
		[SerializeField] private Button _helpBtn;
		[SerializeField] private Image _imageBg;

		[SerializeField] private Sprite _closed;
		[SerializeField] private Sprite _opened;
		
		// ---------------------------------------------------------------------------------
		[SerializeField] private RectTransform _detailPanel;
		[SerializeField] private TMP_Text _detailTMP;
		

		private bool _open;

		private void Awake()
		{
			_helpBtn.onClick.AddListener(SwapSprites);
		}

		private void SwapSprites()
		{
			if (_open)
			{
				_open = false;
				_image.sprite = _closed;
				StartCoroutine(FoldHelp());
			}
			else
			{
				_open = true;
				_image.sprite = _opened;
				StartCoroutine(UnfoldHelp());
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_imageBg.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_imageBg.gameObject.SetActive(false);
		}

		private IEnumerator FoldHelp()
		{
			_detailTMP.gameObject.SetActive(false);
			
			yield return StartCoroutine(
				AnimationsController.ScaleUiElement(_detailPanel, new Vector3(1,0,1), TfMath.EaseLinear, 0.05f));
		}
		
		private IEnumerator UnfoldHelp()
		{
			// despliegue panel
			yield return StartCoroutine(
				AnimationsController.ScaleUiElement(_detailPanel, Vector3.one, TfMath.EaseLinear, 0.05f));
			
			_detailTMP.gameObject.SetActive(true);
		}
	}
}