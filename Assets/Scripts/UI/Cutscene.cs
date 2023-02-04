using System;
using System.Collections;
using Backend.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
	public class Cutscene : MonoBehaviour
	{
		[SerializeField] public Button _clickToShowText;
		[SerializeField] public TMP_Text _pressAnyToSkip;
		[SerializeField] public Image _displayImg;
		[Space, SerializeField] private CutsceneVignette[] _vignettes;
		
		[Serializable]
		public class CutsceneVignette
		{
			public Sprite Sprite;
			[Range(0,5)] public float Duration;
			public AudioType Audio;
			[Range(0.75f, 1.25f)] public float PanInOut;
		}

		private bool _canSkip;

		private void Start()
		{
			_clickToShowText.onClick.AddListener(EnableSkip);
		}

		private void Update()
		{
			if (_canSkip)
			{
				if (Input.anyKeyDown)
				{
					StopAllCoroutines();
					SceneManager.LoadScene("MainMenuScene");
				}
			}
			
		}

		public void Play()
		{
			StartCoroutine(PlayCutscene());
		}
		
		private void EnableSkip()
		{
			_canSkip = true;
			_pressAnyToSkip.gameObject.SetActive(true);
			_pressAnyToSkip.text = StringBank.GetStringRaw("PRESS_ANY_KEY");
		}
		
		private IEnumerator PlayCutscene()
		{
			for (int i = 0; i < _vignettes.Length; i++)
			{
				_displayImg.sprite = _vignettes[i].Sprite;
				
				// Start playing Audio
				
				// Start the pan in/out
				float startScale = _vignettes[i].PanInOut < 1 ? 1f + (1 - _vignettes[i].PanInOut) : 1f;
				IEnumerator panCoroutine = AnimationsController.ScaleUiElement(_displayImg.rectTransform, Vector3.one * startScale,
					_vignettes[i].PanInOut * Vector3.one, TfMath.EaseLinear, _vignettes[i].Duration + 2);
				StartCoroutine(panCoroutine);
				
				if (i == 0)
				{
					// Fade in black
					yield return StartCoroutine(
						AnimationsController.ImageColorFadeIn(_displayImg, TfMath.EaseInOutSine, 0.5f));
				}

				yield return new WaitForSeconds(_vignettes[i].Duration);
				
				if (i == _vignettes.Length-1)
				{
					// Fade out black
					yield return StartCoroutine(
						AnimationsController.ImageColorFadeOut(_displayImg, TfMath.EaseInOutSine, 0.5f));
				}
				
				StopCoroutine(panCoroutine);
			}
			
			SceneManager.LoadScene("MainMenuScene");
			
		}
	}
}