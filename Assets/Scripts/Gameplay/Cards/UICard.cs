using System.Collections;
using Audio;
using Backend.Localization;
using DebugScripts;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Cards
{
    public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UICardData UICardData { get; private set; }
        
        [SerializeField] private Image _baseImage;
        [SerializeField] private Image _ilustration;
        [SerializeField] private Image _cardDetail;
        [SerializeField] private TextMeshProUGUI _cardName;
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _panelRectTransform;
        /// <summary>
        /// Shows when selected.
        /// </summary>
        [SerializeField] private Image _border;
        public Image Border => _border;

        private GraphicRaycaster _tempGraphicRaycaster;
        private Canvas _tempCanvas;

        private IEnumerator _rotateAnim;
        private IEnumerator _levitateAnim;

        public void Init(UICardData uiCardData, Sprite cardBase)
        {
            UICardData = uiCardData;
            _baseImage.sprite = cardBase;
            _baseImage.material = UICardData.CardBaseMat;
            _ilustration.sprite = UICardData.CardSprite;
            _cardDetail.color = UICardData.DetailColor;
            _cardName.text = StringBank.GetStringRaw(UICardData.CardName);
            _button.onClick.AddListener(TaskOnClick);
            
            //TODO uses a temporary canvas to bring the hovered card to the front of the layout group
            // _tempCanvas = gameObject.AddComponent<Canvas>();
            // _tempGraphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            //TODO
            
            DisableButton();
        }

        private void TaskOnClick()
        {
            _border.gameObject.SetActive(true);
            PlacingCardAnimation();
            DisableButton();
            AudioController.Get().PlaySfx(AudioId.SFX_ClickOnCard);
            ObjectCache.Current.PlayerTurnManager.PickUpUICard(this);
        }
        
        private void ShowCard()
        {
            StopAllCoroutines();
            StartCoroutine(AnimationsController.MoveUIElementY(_panelRectTransform, -75f,105f, TfMath.EaseOutQuint, 0.2f));
            StartCoroutine(AnimationsController.ScaleUiElement(_panelRectTransform, Vector3.one, new Vector3(1.3f,1.3f,1.3f), TfMath.EaseOutQuint, 0.2f));
        }

        private void HideCard()
        {
            StopAllCoroutines();
            StartCoroutine(HideCardCoroutine());
        }
        
        private IEnumerator HideCardCoroutine()
        {
            DisableButton();
            StartCoroutine(AnimationsController.MoveUIElementY(_panelRectTransform, 105f,-75f, TfMath.EaseOutQuint, 0.1f));
            yield return (AnimationsController.ScaleUiElement(_panelRectTransform, new Vector3(1.3f,1.3f,1.3f), Vector3.one, TfMath.EaseOutQuint, 0.1f));
            EnableButton();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public TileType GetCardTileType()
        {
            return UICardData.tileId;
        }

        public TileBehaviour GetBehaviourPreview()
        {
            TileBehaviour beh = ObjectCache.Current.TileBehaviourPool.GetNewTile(UICardData.tileId);
            beh.ShowPreview(Color.white);
            return beh;
        }

        public void EnableButton()
        {
            _baseImage.raycastTarget = true;
            _button.interactable = true;
        }

        public void DisableButton()
        {
            _button.interactable = false;
            _baseImage.raycastTarget = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_button.interactable) return;
            
            // AudioController.Get().PlaySfx(AudioId.SFX_HoverCard);

            // // uses a temporary canvas to bring the hovered card to the front of the layout group
            // _tempCanvas = gameObject.AddComponent<Canvas>();
            // _tempCanvas.overrideSorting = true;
            // _tempCanvas.sortingOrder = 1;
            // _tempGraphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            //todo descomentar
            
            _cardName.ForceMeshUpdate();
            
            // if( Tooltip.Instance.GetShowCardTooltip()) Tooltip.Instance.ShowToolTip(UiCardInfo.Description);
            // if (_button.interactable)
                ShowCard();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_button.interactable) return;
            
            //todo descomentar
            // Destroy(_tempGraphicRaycaster);
            // Destroy(_tempCanvas);
            // _tempCanvas.overrideSorting = true; //todo borrar
            
            // if( Tooltip.Instance.GetShowCardTooltip()) Tooltip.Instance.HideToolTip();
            // if (_button.interactable)
                HideCard();
        }

        public void PutBackCard()
        {
            _border.gameObject.SetActive(false);
            StopPlacingCardAnimation();
            //todo descomentar
            // Destroy(_tempGraphicRaycaster);
            // Destroy(_tempCanvas);
            HideCard();
        }
        
        public IEnumerator DestroyCardAnimation()
        {
            Material destroyMat = ObjectCache.Current.CardPool.DestroyCardMat;
            _ilustration.material = destroyMat;
            _baseImage.material = destroyMat;
            _border.gameObject.SetActive(false);
            
            // hide border
            StartCoroutine(AnimationsController.ImageAlphaFadeOut(_cardDetail, TfMath.EaseOutExpo, 0.4f));
            // hide name
            StartCoroutine(AnimationsController.TextFadingOut(_cardName, TfMath.EaseOutExpo, 0.4f));
            
            // animate burn effect
            StartCoroutine(AnimationsController.EaseMaterialFloat(_ilustration.material, "_RevealValue", 1, 0,TfMath.EaseInOutCubic, 0.7f));
            yield return StartCoroutine(AnimationsController.EaseMaterialFloat(_baseImage.material, "_RevealValue", 1, 0,TfMath.EaseInOutCubic, 0.75f));
        }

        private void PlacingCardAnimation()
        {
            _rotateAnim = RotateAnimationCoroutine();
            _levitateAnim = LevitateAnimationCoroutine();
            StartCoroutine(_rotateAnim);
            StartCoroutine(_levitateAnim);
        }

        private IEnumerator RotateAnimationCoroutine()
        {
            while (true)
            {
                float amplitude = Random.Range(0.01f, 0.03f);
                yield return AnimationsController.RotateUIElement(_panelRectTransform, 0.0f, amplitude, TfMath.BellEaseCentered, 2f);
            }
        }
        
        private IEnumerator LevitateAnimationCoroutine()
        {
            while (true)
            {
                yield return AnimationsController.MoveUIElementY(_panelRectTransform, 105f, 120f, TfMath.BellEase, 1.2f);
            }
        }

        private void StopPlacingCardAnimation()
        {
            if (_rotateAnim != null) StopCoroutine(_rotateAnim);
            if (_levitateAnim != null) StopCoroutine(_levitateAnim);
            
            _panelRectTransform.localRotation = Quaternion.identity;
            _panelRectTransform.localPosition = new Vector3(_panelRectTransform.localPosition.x, 105f, 0f);
            
        }

        
        
    }
}
    

