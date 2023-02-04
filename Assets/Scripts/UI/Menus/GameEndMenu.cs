using System.Collections;
using Audio;
using Backend.Localization;
using Controls;
using Cysharp.Text;
using Gameplay;
using TMPro;
using UI.MenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
	public class GameEndMenu : Menu<GameEndMenu>
	{
		public override bool PlayAudioOnOpen => false;
		public override bool PlayAudioOnClose => false;

		[SerializeField] private Image _centralImg;
		
		// Game End menu buttons 
		[Space]
		[SerializeField] private Button _resetRun;
		[SerializeField] private Button _backToTitle;

		[SerializeField] private TextMeshProUGUI _resetRunTMP;
		[SerializeField] private TextMeshProUGUI _backToTitleTMP;
		
		[SerializeField] private TextMeshProUGUI _finalScoreTMP;
		[SerializeField] private TextMeshProUGUI _highScoreTMP;
		[SerializeField] private TextMeshProUGUI _reasonTMP;
		
		// For animations
		[Space]
		[SerializeField] private Image _fadeImg;
		[SerializeField] private RectTransform _border;
		// [SerializeField] private RectTransform _gameOver; // 1,0,1
		[SerializeField] private RectTransform _reasonRect; // 0,0,0
		[SerializeField] private RectTransform _scoreRect; // 0,0,0
		[SerializeField] private RectTransform _highscoreRect; // 0,0,0
		[SerializeField] private RectTransform _retryBtn; // 0,1,1
		[SerializeField] private RectTransform _mainMenuBtn; // 0,1,1
		
		private static string _reasonText;
		private static Sprite _endRunImg;
		
		[SerializeField] private MenuFogTransition _transition;

		public static void Config(string reasonText, Sprite endRunImg)
		{
			_reasonText = reasonText;
			_endRunImg = endRunImg;
		}
		
		private void Start()
		{
			// Game Over menu buttons
			_resetRun.onClick.AddListener(ResetRun);
			_backToTitle.onClick.AddListener(BackToTitle);
			
			// _finalScoreTMP.text = StringBank.GetStringRaw("TITLE_SCORE");
			// _highScoreTMP.text = StringBank.GetStringRaw("LEADERBOARDS_BEST_SCORE");
			// _reasonTMP.text = StringBank.GetStringRaw("GAME_OVER_OUT_OF_CARDS");
			
			_resetRunTMP.text = StringBank.GetStringRaw("MENU_ACTION_RESTART");
			_backToTitleTMP.text = StringBank.GetStringRaw("MENU_MAIN_MENU");

			_fadeImg.color = new Color(0, 0, 0, 0);
			
			_transition.Init(true);
			
			StartCoroutine(OpenAnimationCoroutine());
		}
		
		private IEnumerator OpenAnimationCoroutine()
		{
			// blur
			StartCoroutine(AnimationsController.EaseDoF(ObjectCache.Current.CameraControl.DepthOfField, 1.5f, 0.0f,
				TfMath.EaseOutCirc, duration: 0.35f));
			// hacer zoom en marco
			StartCoroutine(AnimationsController.ScaleUiElement(_border, 0.8f, TfMath.EaseOutExpo, duration: 0.25f));
			yield return new WaitForSeconds(0.1f);
			// // aparece título
			// StartCoroutine(AnimationsController.ScaleUiElement(_gameOver, Vector3.one, TfMath.EaseOutElastic, duration: 0.35f));
			// yield return new WaitForSeconds(0.35f);
			// aparece reason
			StartCoroutine(AnimationsController.ScaleUiElement(_reasonRect, Vector3.one, TfMath.EaseOutExpo, duration: 0.2f));
			yield return new WaitForSeconds(0.35f);
			// aparece score
			StartCoroutine(AnimationsController.ScaleUiElement(_scoreRect, Vector3.one, TfMath.EaseOutExpo, duration: 0.15f));
			yield return new WaitForSeconds(0.35f);
			// aparece highscore
			StartCoroutine(AnimationsController.ScaleUiElement(_highscoreRect, Vector3.one, TfMath.EaseOutExpo, duration: 0.15f));
			yield return new WaitForSeconds(0.75f);
			// crecen los buttons
			StartCoroutine(AnimationsController.ScaleUiElement(_retryBtn, Vector3.one, TfMath.EaseOutElastic, duration: 0.2f));
			StartCoroutine(AnimationsController.ScaleUiElement(_mainMenuBtn, Vector3.one, TfMath.EaseOutElastic, duration: 0.2f));
			yield return new WaitForSeconds(0.2f);
			
			GameControls.EnableControls(true);
			yield return null;
		}
        
		public static void Show()
		{
			_Open();
		}

		protected override void OnMenuOpen()
		{
			base.OnMenuOpen();
			_centralImg.sprite = _endRunImg;
			Reason();
			
			FinalScore();
			HighScore();
			
			GameManager.Get().SavePlayerStats();
			// todo RunManager = null
			GameManager.Get().ClearRun();
		}

		public override void OnBackPressed()
		{
			// BackToTitle();
		}

		private void ResetRun()
		{
			StartCoroutine(OnResetCoroutine());
		}
		
		private IEnumerator OnResetCoroutine()
		{
			GameControls.EnableControls(false);
			yield return CloseAnimationCoroutine();
			yield return StartCoroutine(_transition.Closes());
			AudioController.Get().StopAmbience(PlaylistId.PL_AM_WORLD1,true);		 // todo refact.
			AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_MENU,false,1f);
			AudioController.Get().PlayAmbience(PlaylistId.PL_AM_MENU,false,1f);
			SceneManager.LoadScene("RunSetUpScene");
			base.OnBackPressed();
		}

		private void BackToTitle()
		{
			StartCoroutine(OnBackToTitleCoroutine());
		}
		
		private IEnumerator OnBackToTitleCoroutine()
		{
			GameControls.EnableControls(false);
			yield return CloseAnimationCoroutine();
			yield return StartCoroutine(_transition.Closes());
			AudioController.Get().StopAmbience(PlaylistId.PL_AM_WORLD1,true);		 // todo refact.
			AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_MENU,false,1f);
			AudioController.Get().PlayAmbience(PlaylistId.PL_AM_MENU,false,1f);
			SceneManager.LoadScene("MainMenuScene");
			base.OnBackPressed();
		}

		private IEnumerator CloseAnimationCoroutine()
		{
			// fade to black
			yield return AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
		}

		private void FinalScore()
		{
			_finalScoreTMP.text = ZString.Concat(StringBank.GetStringRaw("GAME_OVER_SCORE"), ": ", GameManager.Get().RunManager.GlobalScore);
		}
		
		private void HighScore()
		{
			_highScoreTMP.text = ZString.Concat(StringBank.GetStringRaw("GAME_OVER_HIGHSCORE"), " ", GameManager.Get().RunManager.GetHighScore());
		}
		
		private void Reason()
		{
			_reasonTMP.text = _reasonText;
		}
	}
}