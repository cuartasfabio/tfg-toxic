using System.Collections;
using Audio;
using Backend.Localization;
using Controls;
using TMPro;
using UI.MenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
    public class RunConfigMenu : Menu<RunConfigMenu>
    {
        [SerializeField] private TMP_Text _runSettingsTxt;

        public override bool PlayAudioOnOpen => false;
        public override bool PlayAudioOnClose => false;

        [SerializeField] private GameModeButton _runMode;
        [SerializeField] private GameModeButton _endlessMode;
        
        [SerializeField] private Button _playBtn;
        
        [SerializeField] private Image _fadeImg;
        
        [SerializeField] private MenuFogTransition _transition;
        
        
        private void Start()
        {
            _runMode.Init();
            _endlessMode.Init();
            
            _playBtn.onClick.AddListener(Play);
            _runSettingsTxt.text = StringBank.GetStringRaw("RUN_SETTINGS");
            
            _transition.Init(false);
            
            StartCoroutine(OnShowCoroutine());
        }
        
        private IEnumerator OnShowCoroutine()
        {
            yield return AnimationsController.ImageAlphaFadeOut(_fadeImg, TfMath.EaseOutExpo, 0.2f);
            yield return StartCoroutine(_transition.Opens());
            GameControls.EnableControls(true);
        }
        
        private void Play()
        {
            StartCoroutine(OnPlayCoroutine());
        }
        
        private IEnumerator OnPlayCoroutine()
        {
            GameControls.EnableControls(false);
            
            AudioController.Get().StopSoundtrack(PlaylistId.PL_ST_MENU, true);
            AudioController.Get().StopAmbience(PlaylistId.PL_AM_MENU, true);
            AudioController.Get().PlaySfx(AudioId.SFX_RunStart, true, 5f);
            AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_WORLD1, true, 10f);
            AudioController.Get().PlayAmbience(PlaylistId.PL_AM_WORLD1, true, 1f);
            
            yield return StartCoroutine(_transition.Closes());
            yield return AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
            yield return null;
            
            StartCoroutine(LoadAsyncGameScene());
            
            _Close();
        }
        
        private IEnumerator LoadAsyncGameScene()
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync("GameScene");

            while (!asyncOp.isDone)
            {
                yield return null;
            }
        }
        
        public static void Show()
        {
            _Open();
        }
        
        public override void OnBackPressed()
        {
            StartCoroutine(OnBackPressedCoroutine());
        }

        private IEnumerator OnBackPressedCoroutine()
        {
            GameControls.EnableControls(false);
            yield return StartCoroutine(_transition.Closes());
            yield return AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
            SceneManager.LoadScene("MainMenuScene");
            _Close();
        }
        
    }
}