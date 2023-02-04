using Controls;
using Gameplay;
using Scenes;
using UI.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGames
{
	public class PauseButton: MonoBehaviour
	{
		[SerializeField] private Button _button;

		private void Start()
		{
			_button.onClick.AddListener(OpenPause);
		}

		private void OpenPause()
		{
			if (GameControls.AreControlsEnabled())
			{
				GameSceneManager.Current.Pause();
				StartCoroutine(ObjectCache.Current.InGameUI.Hide(PauseMenu.Show));
			}
		}
	}
}