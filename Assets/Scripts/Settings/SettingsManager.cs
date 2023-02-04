using Audio;
using Backend.Localization;
using Backend.Persistence;
using MbsUnity.Runtime.Localization;
using UnityEngine;

namespace Settings
{
	public class SettingsManager : IPersistable
	{
		public struct AllowedResolution
		{
			public int Width;
			public int Height;
		}

		public static readonly AllowedResolution[] AllowedResolutions =
		{
			new AllowedResolution {Width = 1280, Height = 720},
			new AllowedResolution {Width = 1366, Height = 768},
			new AllowedResolution {Width = 1920, Height = 1080},
			new AllowedResolution {Width = 2560, Height = 1080},
			new AllowedResolution {Width = 2560, Height = 1440},
			new AllowedResolution {Width = 3840, Height = 2160},
		};

		public static readonly LanguageIso[] AllowedLanguages =
		{
			LanguageIso.En,
			LanguageIso.Es
		};
		
		
		public SettingsStruct DefaultSettings { get; }
		public SettingsStruct CurrentSettings;

		public SettingsManager()
		{
			DefaultSettings = new SettingsStruct 
			{ 
				Language = 0, // index of AllowedLanguages
				MusicVolume = 0.5f,
				AmbienceVolume = 0.5f,
				EffectsVolume = 0.5f,
				Fullscreen = true,
				Resolution = 2, // index of AllowedResolutions
				ScreenShake = false
			};

			CurrentSettings = DefaultSettings;

		}
		
		
		public void UpdateLanguage(int lan)
		{
			StringBank.LoadIso(AllowedLanguages[lan]);
			CurrentSettings.Language = lan;
		}

		public void UpdateMusicVol(float vol)
		{
			AudioController.Get().ChangeVolume(AudioController.AudioCategory.SOUNDTRACK, vol);
			CurrentSettings.MusicVolume = vol;
		}
		
		public void UpdateAmbiencesVol(float vol)
		{
			AudioController.Get().ChangeVolume(AudioController.AudioCategory.AMBIENCE, vol);
			CurrentSettings.AmbienceVolume = vol;

		}
		
		public void UpdateEffectsVol(float vol)
		{
			AudioController.Get().ChangeVolume(AudioController.AudioCategory.SFX, vol);
			CurrentSettings.EffectsVolume = vol;

		}

		public void UpdateFullscreen(bool isOn)
		{
			Screen.fullScreen = isOn;
			CurrentSettings.Fullscreen = isOn;

		}

		public void UpdateResolution(int res)
		{
			Screen.SetResolution(AllowedResolutions[res].Width, AllowedResolutions[res].Height, Screen.fullScreen);
			CurrentSettings.Resolution = res;
		}

		public void UpdateScreenShake(bool isOn)
		{
			// todo
			CurrentSettings.ScreenShake = isOn;
		}
		
		public void RefreshCurrentSettings()
		{
			UpdateLanguage(CurrentSettings.Language);
			
			UpdateMusicVol(CurrentSettings.MusicVolume);
			UpdateAmbiencesVol(CurrentSettings.AmbienceVolume);
			UpdateEffectsVol(CurrentSettings.EffectsVolume);
			
			UpdateFullscreen(CurrentSettings.Fullscreen);
			UpdateResolution(CurrentSettings.Resolution);
			UpdateScreenShake(CurrentSettings.ScreenShake);
		}
		
		// ----------------------------------------

		public void RestoreToDefaultSettings()
		{
			CurrentSettings = DefaultSettings;
			RefreshCurrentSettings();
		}

		// ---------------------------------------
		
		public void Save(GameDataWriter writer)
		{
			writer.Write(CurrentSettings.Language);
           
			writer.Write(CurrentSettings.MusicVolume);
			writer.Write(CurrentSettings.AmbienceVolume);
			writer.Write(CurrentSettings.EffectsVolume);
           
			writer.Write(CurrentSettings.Fullscreen);
			writer.Write(CurrentSettings.Resolution);
			writer.Write(CurrentSettings.ScreenShake);
		}

		public void Load(GameDataReader reader)
		{
			CurrentSettings.Language = reader.ReadInt();
			
			CurrentSettings.MusicVolume = reader.ReadFloat();
			CurrentSettings.AmbienceVolume = reader.ReadFloat();
			CurrentSettings.EffectsVolume = reader.ReadFloat();
			
			CurrentSettings.Fullscreen = reader.ReadBool();
			CurrentSettings.Resolution = reader.ReadInt();
			CurrentSettings.ScreenShake = reader.ReadBool();
		}

		
		
	}
}