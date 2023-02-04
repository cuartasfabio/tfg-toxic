using System.Collections;
using System.Collections.Generic;
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
    public class MainMenu : Menu<MainMenu>
    {
        public override bool PlayAudioOnOpen => false;
        public override bool PlayAudioOnClose => false;
        
        // Main menu buttons
        [SerializeField] private Button _newGameBtn;
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _cardListBtn;
        [SerializeField] private Button _optionsBtn;
        [SerializeField] private Button _creditsBtn;
        [SerializeField] private Button _exitBtn;

        private List<Button> _buttons;
        private List<TMP_Text> _buttonsTxt;

        [SerializeField] private TMP_Text _newGameTxt;
        [SerializeField] private TMP_Text _continueTxt;
        [SerializeField] private TMP_Text _cardListTxt;
        [SerializeField] private TMP_Text _optionsTxt;
        [SerializeField] private TMP_Text _creditsTxt;

        [SerializeField] private RectTransform _title;
        
        [SerializeField] private RectTransform _background;

        [SerializeField] private Image _fadeImg;
        
        [SerializeField] private List<RectTransform> _buttonsRects;

        [SerializeField] private MenuFogTransition _transition;
        

        public static void Show()
        {
            _Open();
        }

        public static void Hide()
        {
            _Close();
        }
        
        private void Start()
        {
            _buttons = new List<Button>();
            _buttonsTxt = new List<TMP_Text>();
            
            // Main menu buttons
            _newGameBtn.onClick.AddListener(StartGame);
            _buttons.Add(_newGameBtn);
            _buttonsTxt.Add(_newGameTxt);
            _continueBtn.onClick.AddListener(Continue);
            _buttons.Add(_continueBtn);
            _buttonsTxt.Add(_continueTxt);
            _cardListBtn.onClick.AddListener(CardList);
            _buttons.Add(_cardListBtn);
            _buttonsTxt.Add(_cardListTxt);
            _optionsBtn.onClick.AddListener(Options);
            _buttons.Add(_optionsBtn);
            _buttonsTxt.Add(_optionsTxt);
            _creditsBtn.onClick.AddListener(Credits);
            _buttons.Add(_creditsBtn);
            _buttonsTxt.Add(_creditsTxt);
            _exitBtn.onClick.AddListener(ExitGame);
            // _buttons.Add(_exitBtn);

            // _newGameTxt.text = StringBank.GetStringRaw("MENU_ACTION_NEW_GAME");
            _newGameTxt.text = TfLocalizedTexts.GetStringRaw(_newGameTxt, "MENU_ACTION_NEW_GAME");
            // _continueTxt.text = StringBank.GetStringRaw("MENU_ACTION_CONTINUE");
            _continueTxt.text = TfLocalizedTexts.GetStringRaw(_continueTxt, "MENU_ACTION_CONTINUE");
            // _cardListTxt.text = StringBank.GetStringRaw("CARD_COLLECTION");
            _cardListTxt.text = TfLocalizedTexts.GetStringRaw(_cardListTxt, "CARD_COLLECTION");
            // _optionsTxt.text = StringBank.GetStringRaw("MENU_SETTINGS");
            _optionsTxt.text = TfLocalizedTexts.GetStringRaw(_optionsTxt, "MENU_SETTINGS");
            // _creditsTxt.text = StringBank.GetStringRaw("MENU_CREDITS");
            _creditsTxt.text = TfLocalizedTexts.GetStringRaw(_creditsTxt, "MENU_CREDITS");
            // _exitTxt.text = StringBank.GetStringRaw("MENU_ACTION_EXIT");

            _transition.Init(false);
            
            StartCoroutine(OpenAnimationCoroutine(true));
        }
        
        private void StartGame()
        {
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            yield return StartCoroutine(CloseAnimationCoroutine());
            yield return StartCoroutine(AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.2f));
           
            StartCoroutine(LoadAsyncSetUpScene());
            
            _Close();
            
        }

        private IEnumerator LoadAsyncSetUpScene()
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync("RunSetUpScene");

            while (!asyncOp.isDone)
            {
                yield return null;
            }
        }
        
        private void Continue()
        {
            StartCoroutine(ContinueCoroutine());
        }
        
        private IEnumerator ContinueCoroutine()
        {
            yield return StartCoroutine(CloseAnimationCoroutine());
            yield return StartCoroutine(AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.2f));
           
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
        
        private void CardList()
        {
            StartCoroutine(CardListCoroutine());
        }
        
        private IEnumerator CardListCoroutine()
        {
            yield return StartCoroutine(CloseAnimationCoroutine());
            // yield return new WaitForSeconds(1.5f);
            CardListMenu.Show();
        }
        
        private void Options()
        {
            StartCoroutine(OptionsCoroutine());
        }
        
        private IEnumerator OptionsCoroutine()
        {
            yield return StartCoroutine(CloseAnimationCoroutine());
            // yield return new WaitForSeconds(1.5f);
            OptionsMenu.Show();
        }
        
        private void Credits()
        {
            StartCoroutine(CreditsCoroutine());
        }
        
        private IEnumerator CreditsCoroutine()
        {
            yield return StartCoroutine(CloseAnimationCoroutine());
            CreditsMenu.Show();
        }
        
        private void ExitGame()
        {
            Application.Quit();
        }

        private IEnumerator OpenAnimationCoroutine(bool withFade)
        {
            _continueBtn.interactable = false;
            _continueTxt.color = new Color(1, 1, 1, 0.5f);
            if (GameManager.Get().IsThereASavedRun())
            {
                _continueBtn.interactable = true;
                _continueTxt.color = Color.magenta;
            }
            
            // _fadeImg.color = new Color(0, 0, 0, 0);
            if (withFade)
                yield return AnimationsController.ImageAlphaFadeOut(_fadeImg, TfMath.EaseOutExpo, 0.2f);
            // AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_MENU,false,1f);
            
            // _fogBackgroundUp.anchoredPosition = Vector2.zero;
            // _fogBackgroundDown.anchoredPosition = Vector2.zero;
            // _fogDown.anchoredPosition = Vector2.zero;
            // _fogUp.anchoredPosition = Vector2.zero;
            // _fogRight.anchoredPosition = Vector2.zero;
            // _fogLeft.anchoredPosition = Vector2.zero;
            
            // GameControls.EnableControls(false);
            // StartCoroutine(AnimationsController.ImageFadeOut(_fadeImg, TfMath.EaseOutExpo, 0.5f));
            // StartCoroutine(AnimationsController.MoveUIElement(_background, Vector3.down, 600, TfMath.EaseInOutCubic, duration: 0.5f));
            // StartCoroutine(AnimationsController.ScaleUiElement(_background, 0.8f, TfMath.EaseOutExpo, duration: 0.5f));
            // yield return new WaitForSeconds(0.4f);

            yield return StartCoroutine(_transition.Opens());
            
            // StartCoroutine(AnimationsController.FadeInTexts(_buttonsTxt, TfMath.EaseOutExpo, duration:0.2f));
            // yield return StartCoroutine(AnimationsController.MoveUiElements(_buttonsRects, Vector3.right, 200, TfMath.EaseInOutCubic, duration:0.2f));
            
            // StartCoroutine(AnimationsController.MoveYElementSpring(_title, -350f,8f * Mathf.PI, 0.23f, duration: 0.5f));
            
            // StartCoroutine(AnimationsController.MoveUIElement(_fogRight, Vector3.right, Screen.width,
            //     TfMath.EaseInOutCubic, duration: 0.4f));
            // yield return new WaitForSeconds(0.1f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogLeft, Vector3.left, Screen.width,
            //     TfMath.EaseInOutCubic, duration: 0.5f));
            // yield return new WaitForSeconds(0.55f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogDown, Vector3.down, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.36f));
            // yield return new WaitForSeconds(0.2f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogUp, Vector3.up, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.45f));
            // yield return new WaitForSeconds(0.055f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogBackgroundDown, Vector3.down, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.6f));
            // yield return new WaitForSeconds(0.02f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogBackgroundUp, Vector3.up, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.4f));
            //
            // yield return new WaitForSeconds(0.5f);

            // ActiveButtons(true);
            GameControls.EnableControls(true);
        }

        private IEnumerator CloseAnimationCoroutine()
        {
            GameControls.EnableControls(false);
            // ActiveButtons(false);
            // StartCoroutine(AnimationsController.MoveUIElement(_title, Vector3.up, 350, TfMath.EaseInOutCubic, duration:0.5f));
            
            // StartCoroutine(AnimationsController.MoveUiElements(_buttonsRects, Vector3.left, 200, TfMath.EaseInOutCubic, duration:0.2f));
            // yield return StartCoroutine(AnimationsController.FadeOutTexts(_buttonsTxt, TfMath.EaseOutExpo, duration:0.2f));
            
            yield return StartCoroutine(_transition.Closes());
            
            // StartCoroutine(AnimationsController.ImageFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f));
            // StartCoroutine(AnimationsController.ScaleUiElement(_background, 1.25f, TfMath.EaseInOutCubic, duration: 0.5f));
            // StartCoroutine(AnimationsController.MoveUIElement(_background, Vector3.up, 600, TfMath.EaseInOutCubic, duration: 0.5f));

            // StartCoroutine(AnimationsController.MoveUIElement(_fogRight, Vector3.left, Screen.width,
            //     TfMath.EaseInOutCubic, duration: 0.4f));
            // yield return new WaitForSeconds(0.1f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogLeft, Vector3.right, Screen.width,
            //     TfMath.EaseInOutCubic, duration: 0.5f));
            // yield return new WaitForSeconds(0.55f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogDown, Vector3.up, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.36f));
            // yield return new WaitForSeconds(0.2f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogUp, Vector3.down, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.45f));
            // yield return new WaitForSeconds(0.055f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogBackgroundDown, Vector3.up, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.6f));
            // yield return new WaitForSeconds(0.02f);
            // StartCoroutine(AnimationsController.MoveUIElement(_fogBackgroundUp, Vector3.down, Screen.height,
            //     TfMath.EaseInOutCubic, duration: 0.4f));
            // yield return new WaitForSeconds(1.0f);
            
            // GameControls.EnableControls(true);
        }

        public override void OnReactivate()
        {
            StartCoroutine(OpenAnimationCoroutine(false));
        }

        public override void OnBackPressed()
        {
            // does nothing
        }

    }
}