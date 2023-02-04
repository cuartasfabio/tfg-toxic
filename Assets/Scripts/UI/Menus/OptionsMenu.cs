using System.Collections;
using Backend.Localization;
using Controls;
using Cysharp.Text;
using Gameplay;
using Settings;
using TMPro;
using UI.MenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
    public class OptionsMenu : Menu<OptionsMenu>
    {
        [SerializeField] private TMP_Text _optionsTitleTxt;
        
        // [SerializeField] private Image _fadeImg;
        public override bool PlayAudioOnOpen => false;
        public override bool PlayAudioOnClose => false;

        [SerializeField] private MenuFogTransition _transition;

        [Space] [Header("General Settings")] 
        [SerializeField] private TMP_Dropdown _languageDropdown;

        [Space] [Header("Volume Settings")] 
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _ambienceSlider;
        [SerializeField] private Slider _effectsSlider;
        
        [Space] [Header("Screen Settings")] 
        [SerializeField] private Toggle _fullscreenToggle;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private Toggle _screenShakeToggle;

        [Space] 
        [SerializeField] private Button _restoreBtn;
        // [SerializeField] private Button _applyBtn;

        private SettingsManager _settingsManager;

        
        private void Start()
        {
            _settingsManager = GameManager.Get().SettingsManager;
            
            _optionsTitleTxt.text = StringBank.GetStringRaw("MENU_SETTINGS");
            
            _transition.Init(false);
            
            StartCoroutine(OpenAnimationCoroutine());
            
            // general sett
            _languageDropdown.onValueChanged.AddListener(HandleLanguage);
            // volume sett
            _musicSlider.onValueChanged.AddListener(HandleMusicVol);
            _ambienceSlider.onValueChanged.AddListener(HandleAmbiencesVol);
            _effectsSlider.onValueChanged.AddListener(HandleEffectsVol);
            // screen sett
            _fullscreenToggle.onValueChanged.AddListener(HandleFullscreen);
            _resolutionDropdown.onValueChanged.AddListener(HandleResolution);
            _screenShakeToggle.onValueChanged.AddListener(HandleScreenShake);
            // restoreToDefault button
            _restoreBtn.onClick.AddListener(RestoreDefaultSettings);
            
            // fill languages dropdown
            for (int i = 0; i < SettingsManager.AllowedLanguages.Length; i++)
            {
                _languageDropdown.options.Add(new TMP_Dropdown.OptionData(
                    SettingsManager.AllowedLanguages[i].ToString()));
            }
            
            // fill resolutions dropdown
            for (int i = 0; i < SettingsManager.AllowedResolutions.Length; i++)
            {
                _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(
                    ZString.Concat(SettingsManager.AllowedResolutions[i].Width, "x", SettingsManager.AllowedResolutions[i].Height)));
            }
            
            
            RefreshUI();
        }
        
        private void HandleLanguage(int value)
        {
            _settingsManager.UpdateLanguage(value);
            TfLocalizedTexts.RefreshTexts();
        }
        
        private void HandleMusicVol(float value)
        {
            _settingsManager.UpdateMusicVol(value);
        }
        
        private void HandleAmbiencesVol(float value)
        {
            _settingsManager.UpdateAmbiencesVol(value);
        }
        
        private void HandleEffectsVol(float value)
        {
            _settingsManager.UpdateEffectsVol(value);
        }

        private void HandleFullscreen(bool value)
        {
            _settingsManager.UpdateFullscreen(value);
        }
        
        private void HandleResolution(int value)
        {
            _settingsManager.UpdateResolution(value);
        }

        private void HandleScreenShake(bool value)
        {
            _settingsManager.UpdateScreenShake(value);
        }
        
        private void RestoreDefaultSettings()
        {
            _settingsManager.RestoreToDefaultSettings();
            RefreshUI();
        }

        // ----------------------------------

        private void RefreshUI()
        {
            _languageDropdown.value = _settingsManager.CurrentSettings.Language;

            _musicSlider.value = _settingsManager.CurrentSettings.MusicVolume;
            _ambienceSlider.value = _settingsManager.CurrentSettings.AmbienceVolume;
            _effectsSlider.value = _settingsManager.CurrentSettings.EffectsVolume;
            
            _fullscreenToggle.isOn = _settingsManager.CurrentSettings.Fullscreen;
            _resolutionDropdown.value = _settingsManager.CurrentSettings.Resolution;
            _screenShakeToggle.isOn = _settingsManager.CurrentSettings.ScreenShake;
        }

        

        protected override void OnMenuOpen()
        {
            // StartCoroutine(OpenAnimationCoroutine());
        }
        
        private IEnumerator OpenAnimationCoroutine()
        {
            yield return StartCoroutine(_transition.Opens());
            
            if (SceneManager.GetActiveScene().name == "GameScene")
                yield return StartCoroutine(AnimationsController.EaseDoF(ObjectCache.Current.CameraControl.DepthOfField, 1.5f, 0.0f,
                    TfMath.EaseOutCirc, duration: 0.25f));
            
            GameControls.EnableControls(true);
            yield return null;
        }

        public static void Show()
        {
            _Open();
        }
        
        public override void OnBackPressed()
        {
            GameManager.Get().SaveSettings();
            StartCoroutine(OnBackPressedCoroutine());
        }
        
        private IEnumerator OnBackPressedCoroutine()
        {
            yield return CloseAnimationCoroutine();
            _Close();
        }
        
        private IEnumerator CloseAnimationCoroutine()
        {
            GameControls.EnableControls(false);
            yield return StartCoroutine(_transition.Closes());
        }
        
    }
}