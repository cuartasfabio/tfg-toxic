using System;
using System.Collections;
using Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.InGames
{
	public class InGameUI : MonoBehaviour
	{
		[SerializeField] private Image _fadeImg;
		[SerializeField] private RectTransform _lowerBar;
		[SerializeField] private RectTransform _questPanel;
		[SerializeField] private RectTransform _helpPanel;
		[SerializeField] private MenuFogTransition _menuFogTransition;

		private void Start()
		{
			_menuFogTransition.Init(false);
		}

		public IEnumerator Open()
		{
			yield return StartCoroutine(_menuFogTransition.Opens());
			StartCoroutine(Show());
			yield return StartCoroutine(AnimationsController.ImageAlphaFadeOut(_fadeImg, TfMath.EaseOutExpo, 0.5f));
		}
		
		public IEnumerator Close(Action doLast = null)
		{
			yield return Hide();
			yield return StartCoroutine(_menuFogTransition.Closes());
			if (doLast != null) doLast();
			// yield return(AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f));
		}

		public IEnumerator Hide(Action doLast = null)
		{
			GameControls.EnableControls(false);
			StartCoroutine(AnimationsController.MoveUIElementY(_helpPanel,-20, 200, TfMath.EaseOutQuint, duration:0.15f));
			StartCoroutine(AnimationsController.MoveUIElementY(_questPanel,-50, 300, TfMath.EaseOutQuint, duration:0.15f));
			yield return StartCoroutine(AnimationsController.MoveUIElementY(_lowerBar,0, -300, TfMath.EaseOutQuint, duration:0.15f));
			if (doLast != null) doLast();
		}
       
		public IEnumerator Show(Action doLast = null)
		{
			StartCoroutine(AnimationsController.MoveUIElementY(_helpPanel,200, -20, TfMath.EaseOutQuint, duration:0.15f));
			StartCoroutine(AnimationsController.MoveUIElementY(_questPanel,300, -50, TfMath.EaseOutQuint, duration:0.15f));
			yield return StartCoroutine(AnimationsController.MoveUIElementY(_lowerBar,-300, 0, TfMath.EaseOutQuint, duration:0.15f));
			if (doLast != null) doLast();
			GameControls.EnableControls(true);
		}
	}
}