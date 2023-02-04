using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
	public class TestVolumeSliders : MonoBehaviour
	{
		[SerializeField] private Slider _music;
		[SerializeField] private Slider _ambience;
		[SerializeField] private Slider _sfx;

		public void Awake()
		{
			_music.onValueChanged.AddListener(delegate { ValueChangeMusic(); });
			_ambience.onValueChanged.AddListener(delegate { ValueChangeAmbience(); });
			_sfx.onValueChanged.AddListener(delegate { ValueChangeSfx(); });
		}

		private void ValueChangeMusic()
		{
			AudioController.Get().ChangeVolume(AudioController.AudioCategory.SOUNDTRACK, _music.value);
		}
		
		private void ValueChangeAmbience()
		{
			AudioController.Get().ChangeVolume(AudioController.AudioCategory.AMBIENCE, _ambience.value);
		}
		
		private void ValueChangeSfx()
		{
			AudioController.Get().ChangeVolume(AudioController.AudioCategory.SFX, _sfx.value);
		}
	}
}