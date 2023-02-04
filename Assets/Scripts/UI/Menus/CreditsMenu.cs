using System.Collections;
using Backend.Localization;
using Controls;
using TMPro;
using UI.MenuSystem;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
	public class CreditsMenu: Menu<CreditsMenu>
	{
		[SerializeField] private TMP_Text _creditsTitleTxt;
		
		[SerializeField] private Image _fadeImg;
		
		public override bool PlayAudioOnOpen => false;
		public override bool PlayAudioOnClose => false;
        
		/*[SerializeField] private Button _button;
		
		private void Start()
		{
		    // Control menu buttons
		    _button.onClick.AddListener(DoSomething);
		}
		
		private void DoSomething()
		{
		
		}*/
		
		private void Start()
		{
			_creditsTitleTxt.text = StringBank.GetStringRaw("MENU_CREDITS");
			
			StartCoroutine(OnShowCoroutine());
		}
		
		private IEnumerator OnShowCoroutine()
		{
			yield return AnimationsController.ImageAlphaFadeOut(_fadeImg, TfMath.EaseOutExpo, 0.5f);
			GameControls.EnableControls(true);
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
			yield return AnimationsController.ImageAlphaFadeIn(_fadeImg, TfMath.EaseOutExpo, 0.5f);
			_Close();
		}
	}
}