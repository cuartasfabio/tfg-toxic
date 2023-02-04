using System.Collections;
using Backend.Localization;
using Controls;
using TMPro;
using UI.Menus.CardCollections;
using UI.MenuSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class CardListMenu : Menu<CardListMenu>
    {
        // [SerializeField] private TMP_Text _cardsTitleTxt;
        [SerializeField] private Image _fadeImg;

        [SerializeField] private CardCollection _cardCollection;

        public override bool PlayAudioOnOpen => false;
        public override bool PlayAudioOnClose => false;

        [SerializeField] private MenuFogTransition _transition;

        private void Start()
        {
            // _cardsTitleTxt.text = StringBank.GetStringRaw("CARD_COLLECTION");
            
            _transition.Init(false);
            
            StartCoroutine(OnShowCoroutine());
        }
        
        private IEnumerator OnShowCoroutine()
        {
            // yield return (AnimationsController.ImageFadeOut(_fadeImg, TfMath.EaseOutExpo, 0.5f));
            // yield return null;

            _cardCollection.FillCollection();
                
            yield return StartCoroutine(_transition.Opens());
            GameControls.EnableControls(true);
        }
        
        public static void Show()
        {
            _Open();
        }

        public override void OnBackPressed()
        {
            GameControls.EnableControls(false);
            StartCoroutine(OnBackPressedCoroutine());
        }
        
        private IEnumerator OnBackPressedCoroutine()
        {
            GameControls.EnableControls(false);
            // yield return AnimationsController.ImageFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
            // yield return null;
            yield return StartCoroutine(_transition.Closes());
            _Close();
        }
        
        // private void Start()
        // {
        //     // Options menu buttons
        //     _settings.onClick.AddListener(Settings);
        //     _controls.onClick.AddListener(Controls);
        // }
        
    }
}