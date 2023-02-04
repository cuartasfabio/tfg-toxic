using System.Collections;
using System.Collections.Generic;
using Audio;
using Backend.Localization;
using Controls;
using Gameplay;
using Scenes;
using TMPro;
using UI.MenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
    public class PauseMenu : Menu<PauseMenu>
    {
        // [SerializeField] private TMP_Text _pauseTitleTxt;
        
        [SerializeField] private Button _resetBtn;
        [SerializeField] private Button _optionsBtn;
        [SerializeField] private Button _backTitleBtn;
        
        [SerializeField] private TMP_Text _resetTxt;
        [SerializeField] private TMP_Text _optionsTxt;
        [SerializeField] private TMP_Text _backTitleTxt;
        
        public override bool PlayAudioOnOpen => false;
        public override bool PlayAudioOnClose => false;
        
        // Pause menu buttons
        [SerializeField] private Button _resetRun;
        [SerializeField] private Button _options;
        [SerializeField] private Button _backToTitle;
        
        [SerializeField] private Image _fadeImg;
        // [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _border;
        [SerializeField] private RectTransform _panelButtons;
        [SerializeField] private List<RectTransform> _buttons;
        
        [SerializeField] private MenuFogTransition _transition;

        private void Start()
        {
            // Pause menu buttons
            _resetRun.onClick.AddListener(ResetRun);
            _options.onClick.AddListener(Options);
            _backToTitle.onClick.AddListener(BackToTitle);
            
            // _pauseTitleTxt.text = StringBank.GetStringRaw("TITLE_PAUSED");
            _resetTxt.text = StringBank.GetStringRaw("MENU_ACTION_RESTART");
            _optionsTxt.text = StringBank.GetStringRaw("MENU_SETTINGS");
            _backTitleTxt.text = StringBank.GetStringRaw("MENU_MAIN_MENU");

            _fadeImg.color = new Color(0, 0, 0, 0);
            
            _transition.gameObject.SetActive(true);
            _transition.Init(true);

            StartCoroutine(OpenAnimationCoroutine());
        }
        
        public static void Show()
        {
            _Open();
        }

        // protected override void OnMenuOpen()
        // {
        //     StartCoroutine(OpenAnimationCoroutine());
        // }
        
        public override void OnReactivate()
        {
            StartCoroutine(_transition.Opens());
            StartCoroutine(OpenAnimationCoroutine());
        }
        
        private IEnumerator OpenAnimationCoroutine()
        {
            // AudioController.Get().PlaySfx(AudioId.SFX_Pause); from in game
            
            // blur
            StartCoroutine(AnimationsController.EaseDoF(ObjectCache.Current.CameraControl.DepthOfField, 1.5f, 0.0f,
                TfMath.EaseOutCirc, duration: 0.35f));
            // hacer zoom en marco
            StartCoroutine(AnimationsController.ScaleUiElement(_border, 0.8f, TfMath.EaseOutExpo, duration: 0.25f));
            
            // aparece título
            // StartCoroutine(AnimationsController.ScaleUiElement(_title, Vector3.one, TfMath.EaseOutExpo, duration: 0.2f));
            
            // crece el panel
            // StartCoroutine(AnimationsController.ScaleUiElement(_panelButtons, Vector3.one, TfMath.EaseOutElastic, duration: 0.2f));
            
            // crecen los buttons
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < _buttons.Count; i++)
            {
                StartCoroutine(AnimationsController.ScaleUiElement(_buttons[i], Vector3.one, TfMath.EaseOutElastic, duration: 0.1f));
                yield return new WaitForSeconds(0.025f);
            }
            
            GameControls.EnableControls(true);
        }

        public override void OnBackPressed()
        {
            StartCoroutine(OnBackPressedCoroutine());
        }

        private IEnumerator OnBackPressedCoroutine()
        {
            yield return CloseAnimationCoroutine();
            yield return ObjectCache.Current.InGameUI.Show(base.OnBackPressed);
            GameSceneManager.Current.Resume();
        }
        
        private IEnumerator CloseAnimationCoroutine()
        {
            AudioController.Get().PlaySfx(AudioId.SFX_Resume, false, 0.5f);
            
            GameControls.EnableControls(false);
            
            // decrecen los buttons
            for (int i = _buttons.Count -1; i >= 0; i--)
            {
                StartCoroutine(AnimationsController.ScaleUiElement(_buttons[i], Vector3.zero, TfMath.EaseOutExpo, duration: 0.1f));
                yield return new WaitForSeconds(0.025f);
            }
            
            // decrece el panel
            // yield return(AnimationsController.ScaleUiElement(_panelButtons, Vector3.zero, TfMath.EaseInBack, duration: 0.15f));
            
            // blur
            StartCoroutine(AnimationsController.EaseDoF(ObjectCache.Current.CameraControl.DepthOfField, 0.0f, 1.5f,
                TfMath.EaseOutCirc, duration: 0.25f));
            
            // desaparece título
            // StartCoroutine(AnimationsController.ScaleUiElement(_title, Vector3.zero, TfMath.EaseOutExpo, duration: 0.2f));
            
            // deshacer zoom en marco
            yield return AnimationsController.ScaleUiElement(_border, 1.25f, TfMath.EaseInOutCubic, duration: 0.25f );
            
        }

        private void ResetRun()
        {
            StartCoroutine(OnResetCoroutine());
        }
        
        private IEnumerator OnResetCoroutine()
        {
            GameControls.EnableControls(false);
            yield return StartCoroutine(_transition.Closes());
            yield return AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
            AudioController.Get().StopSoundtrack(PlaylistId.PL_ST_WORLD1,true); // todo refact.
            AudioController.Get().StopAmbience(PlaylistId.PL_AM_WORLD1,true);
            AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_MENU,false,1f);
            AudioController.Get().PlayAmbience(PlaylistId.PL_AM_MENU,false,1f);
            SceneManager.LoadScene("RunSetUpScene");
            base.OnBackPressed();

        }

        private void Options()
        {
            StartCoroutine(OnOptionsCoroutine());
        }
        
        private IEnumerator OnOptionsCoroutine()
        {
            GameControls.EnableControls(false);
            // yield return AnimationsController.ImageFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
            yield return CloseAnimationCoroutine();
            yield return StartCoroutine(_transition.Closes());
            OptionsMenu.Show();
        }
        
        private void BackToTitle()
        {
            StartCoroutine(OnBackToTitleCoroutine());
        }
        
        private IEnumerator OnBackToTitleCoroutine()
        {
            GameControls.EnableControls(false);
            yield return StartCoroutine(_transition.Closes());
            yield return AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
            AudioController.Get().StopSoundtrack(PlaylistId.PL_ST_WORLD1,true);     // todo refact.
            AudioController.Get().StopAmbience(PlaylistId.PL_AM_WORLD1,true);
            AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_MENU,false,1f);
            AudioController.Get().PlayAmbience(PlaylistId.PL_AM_MENU,false,1f);
            SceneManager.LoadScene("MainMenuScene");
            base.OnBackPressed();
        }
        
        

        
        
    }
}