using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
	public class MenuFogTransition : MonoBehaviour
	{
		[SerializeField] private RectTransform _fogRight;
		[SerializeField] private RectTransform _fogLeft;
		[SerializeField] private RectTransform _fogDown;
		[SerializeField] private RectTransform _fogUp;
		[SerializeField] private RectTransform _fogBackgroundDown;
		[SerializeField] private RectTransform _fogBackgroundUp;
		[Space] [SerializeField] private Image _loading;

		private float _topAnchor;
		private float _downAnchor;
		private float _leftAnchor;
		private float _rightAnchor;

		private float _speedMul;

		private IEnumerator _loadingCoroutine;

		public void Init(bool opened) // todo when resolution is changed in settings, anchors doesn't change...
		{
			_speedMul = 1.5f;
			
			// _topAnchor = Screen.height + 250;
			// _downAnchor = -Screen.height - 250;
			// _leftAnchor = -Screen.width - 100;
			// _rightAnchor = Screen.width + 100;

			// _topAnchor = Screen.height * 1.1f;
			// _downAnchor = -Screen.height * 1.1f;
			// _leftAnchor = -Screen.width * 1.1f;
			// _rightAnchor = Screen.width * 1.1f;
			
			_topAnchor = 1080 * 1.1f;
			_downAnchor = -1080 * 1.1f;
			_leftAnchor = -1920 * 1.1f;
			_rightAnchor = 1920 * 1.1f;
			
			
			
			if (opened)
			{
				PositionOpen();
				HideSprites(false);
			}
			else
			{
				PositionClosed();
				HideSprites(true);
			}
		}
		
		public IEnumerator Opens()
		{
			HideSprites(true);
			
			AudioController.Get().PlaySfx(AudioId.SFX_FogTransitionOut,false, 0.2f);
			
			StartCoroutine(AnimationsController.MoveUIElementY(_fogBackgroundUp, 0, _topAnchor,
				TfMath.EaseInCubic, duration: 0.22f * _speedMul));
			yield return new WaitForSeconds(0.03f * _speedMul);
			StartCoroutine(AnimationsController.MoveUIElementY(_fogUp, 0, _topAnchor,
				TfMath.EaseInCubic, duration: 0.15f * _speedMul));
			yield return new WaitForSeconds(0.05f * _speedMul);
			
			StartCoroutine(AnimationsController.MoveUIElementY(_fogBackgroundDown, 0, _downAnchor,
				TfMath.EaseInCubic, duration: 0.22f * _speedMul));
			yield return new WaitForSeconds(0.03f * _speedMul);
			yield return StartCoroutine(AnimationsController.MoveUIElementY(_fogDown, 0, _downAnchor,
				TfMath.EaseInCubic, duration: 0.15f * _speedMul));
			
			StartCoroutine(AnimationsController.MoveUIElementX(_fogRight, 0, _rightAnchor,
				TfMath.EaseInCubic, duration: 0.1f * _speedMul));
			yield return new WaitForSeconds(0.05f * _speedMul);
			yield return StartCoroutine(AnimationsController.MoveUIElementX(_fogLeft, 0, _leftAnchor,
				TfMath.EaseInCubic, duration: 0.15f * _speedMul));
			// yield return new WaitForSeconds(0.05f * _speedMul);
			
			// PositionOpen();
			// gameObject.SetActive(false);
			
			HideSprites(false);
		}

		public IEnumerator Closes()
		{
			HideSprites(true);
			
			PositionOpen();
			// gameObject.SetActive(true);
			
			AudioController.Get().PlaySfx(AudioId.SFX_FogTransitionIn,false, 0.2f);
			
			StartCoroutine(AnimationsController.MoveUIElementX(_fogRight, _rightAnchor, 0,
				TfMath.EaseOutCubic, duration: 0.12f * _speedMul));
			yield return new WaitForSeconds(0.03f * _speedMul);
			StartCoroutine(AnimationsController.MoveUIElementX(_fogLeft, _leftAnchor, 0,
				TfMath.EaseOutCubic, duration: 0.15f * _speedMul));
			// yield return new WaitForSeconds(0.05f * _speedMul);
			
			StartCoroutine(AnimationsController.MoveUIElementY(_fogDown, _downAnchor, 0,
				TfMath.EaseOutCubic, duration: 0.15f * _speedMul));
			yield return new WaitForSeconds(0.03f * _speedMul);
			StartCoroutine(AnimationsController.MoveUIElementY(_fogBackgroundDown, _downAnchor, 0,
				TfMath.EaseOutCubic, duration: 0.22f * _speedMul));
			yield return new WaitForSeconds(0.05f * _speedMul);
			
			StartCoroutine(AnimationsController.MoveUIElementY(_fogUp, _topAnchor, 0,
				TfMath.EaseOutCubic, duration: 0.15f * _speedMul));
			yield return new WaitForSeconds(0.03f * _speedMul);
			yield return StartCoroutine(AnimationsController.MoveUIElementY(_fogBackgroundUp, _topAnchor, 0,
				TfMath.EaseOutCubic, duration: 0.22f * _speedMul));

			HideSprites(false);
			
		}

		public void StartLoading()
		{
			_loadingCoroutine = LoadingAnim();
			StartCoroutine(_loadingCoroutine);
		}

		private IEnumerator LoadingAnim()
		{
			while (true)
			{
				yield return StartCoroutine(AnimationsController.EaseImageFillAmount(_loading,0,1,TfMath.EaseLinear,0.25f));
				_loading.fillClockwise = false;
				yield return StartCoroutine(AnimationsController.EaseImageFillAmount(_loading,1,0,TfMath.EaseLinear,0.25f));
				_loading.fillClockwise = true;
				_loading.fillAmount = 0;
				yield return new WaitForSeconds(0.25f);
			}
		}

		public void StopLoading()
		{
			StopCoroutine(_loadingCoroutine);
			_loading.fillClockwise = true;
			_loading.fillAmount = 0;
		}


		private void HideSprites(bool hide)
		{
			_fogBackgroundUp.gameObject.SetActive(hide);
			_fogBackgroundDown.gameObject.SetActive(hide);
			_fogDown.gameObject.SetActive(hide);
			_fogUp.gameObject.SetActive(hide);
			_fogRight.gameObject.SetActive(hide);
			_fogLeft.gameObject.SetActive(hide);
		}

		private void PositionOpen()
		{
			_fogBackgroundUp.anchoredPosition = Vector2.up * _topAnchor;
			_fogBackgroundDown.anchoredPosition = Vector2.down * _topAnchor;
			_fogDown.anchoredPosition = Vector2.down * _topAnchor;
			_fogUp.anchoredPosition = Vector2.up * _topAnchor;
			_fogRight.anchoredPosition = Vector2.right * _rightAnchor;
			_fogLeft.anchoredPosition = Vector2.left * _rightAnchor;
		}

		private void PositionClosed()
		{
			_fogBackgroundUp.anchoredPosition = Vector2.zero;
			_fogBackgroundDown.anchoredPosition = Vector2.zero;
			_fogDown.anchoredPosition = Vector2.zero;
			_fogUp.anchoredPosition = Vector2.zero;
			_fogRight.anchoredPosition = Vector2.zero;
			_fogLeft.anchoredPosition = Vector2.zero;
		}
	}
}