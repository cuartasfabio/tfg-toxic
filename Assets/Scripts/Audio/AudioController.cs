using System;
using System.Collections;
using MbsUnity.Runtime.Common;
using UnityEngine;

namespace Audio
{
	public class AudioController : SingletonBehaviour<AudioController>
	{
		[SerializeField] private bool _debug;

		[Space(10)] [SerializeField] private AudioData audioData;
		
		[Space(10)]
		
		[SerializeField] private AudioSource _soundtrackSrc;
		[SerializeField] private AudioSource _ambienceSrc;
		[SerializeField] private AudioSource _sfxSrc;
		[SerializeField] private SfxSourcePool _sourcesAtCoordsPool;

		[Space(10)] [SerializeField, Range(0.1f,2f)] private float _fadeDuration;

		private Hashtable _audioTable; // relationship between audio types (key) and audios (value)
		private Hashtable _jobTable; // relationship between audio types (key) and jobs (value) (Coroutine,IEnumerator)
		private Hashtable _playlistTable;

		[System.Serializable]
		public class AudioObject
		{
			public AudioId Id;
			public AudioClip Clip;
			[Range(0.0f,1.0f)] public float Volume = 1f;
			
		}

		[Serializable]
		public class AudioPlaylist
		{
			public PlaylistId Id;
			public AudioObject[] Tracks;
			public bool Loops;
		}

		private class AudioJob
		{
			public AudioAction Action { get; }
			public AudioId Id { get; }
			public bool Fade { get; }
			public float Delay { get; }
			public float MainVolume { get; }
			public AudioSource Source { get; }

			public AudioJob(AudioAction action, AudioId id, float mainVolume, AudioSource source, bool fade, float delay)
			{
				Action = action;
				Id = id;
				Fade = fade;
				Delay = delay;
				MainVolume = mainVolume;
				Source = source;
			}
		}
		
		private class PlaylistJob
		{
			public AudioAction Action { get; }
			public PlaylistId Id { get; }
			public bool Fade { get; }
			public bool FadeTracks { get; }
			public float InitialDelay { get; }
			public float MainVolume { get; }
			public AudioSource Source { get; }

			public PlaylistJob(AudioAction action, PlaylistId id, float mainVolume, AudioSource source, bool fade, bool fadeTracks, float initialDelay)
			{
				Action = action;
				Id = id;
				Fade = fade;
				FadeTracks = fadeTracks;
				InitialDelay = initialDelay;
				MainVolume = mainVolume;
				Source = source;
			}
		}

		private enum AudioAction
		{
			PLAY,
			STOP,
			RESTART
		}

		public enum AudioCategory
		{
			SOUNDTRACK,
			AMBIENCE,
			SFX
		}
		
		#region Unity Functions

		public override void Awake()
		{
			base.Awake();
			Configure();
			
			_stVolume = 1f;
			_amVolume = 1f;
			_sfxVolume = 1f;
		}

		private void OnDisable()
		{
			Dispose();
		}

		#endregion

		#region Public Functions

		
		public void PlaySfx(AudioId id, bool fade=false, float delay=0.0f)
		{
			AddJob(new AudioJob(AudioAction.PLAY, id, _sfxVolume, _sourcesAtCoordsPool.Request2DAudioSource(), fade, delay));
		}

		public void StopSfx(AudioId id, bool fade=false, float delay=0.0f)
		{
			AddJob(new AudioJob(AudioAction.STOP, id, _sfxVolume, _sfxSrc, fade, delay));
		}
		
		public void RestartSfx(AudioId id, bool fade=false, float delay=0.0f)
		{
			AddJob(new AudioJob(AudioAction.RESTART, id, _sfxVolume, _sfxSrc, fade, delay));
		}
		
		// soundtrack functions

		public void PlaySoundtrack(PlaylistId id, bool fade=false, float delay = 0.0f)
		{
			AddPlaylistJob(new PlaylistJob(AudioAction.PLAY, id, _stVolume, _soundtrackSrc, fade, false, delay));
		}
		
		public void StopSoundtrack(PlaylistId id, bool fade=false, float delay = 0.0f)
		{
			AddPlaylistJob(new PlaylistJob(AudioAction.STOP, id, _stVolume, _soundtrackSrc, fade, false, delay));
		}
		
		public void RestartSoundtrack(PlaylistId id, bool fade=false, float delay = 0.0f)
		{
			AddPlaylistJob(new PlaylistJob(AudioAction.RESTART, id, _stVolume, _soundtrackSrc, fade, false, delay));
		}
		
		// ambience functions
		
		public void PlayAmbience(PlaylistId id, bool fade=false, float delay = 0.0f)
		{
			AddPlaylistJob(new PlaylistJob(AudioAction.PLAY, id, _amVolume, _ambienceSrc, fade, false, delay));
		}
		
		public void StopAmbience(PlaylistId id, bool fade=false, float delay = 0.0f)
		{
			AddPlaylistJob(new PlaylistJob(AudioAction.STOP, id, _amVolume, _ambienceSrc, fade, false, delay));
		}
		
		public void RestartAmbience(PlaylistId id, bool fade=false, float delay = 0.0f)
		{
			AddPlaylistJob(new PlaylistJob(AudioAction.RESTART, id, _amVolume, _ambienceSrc, fade, false, delay));
		}
		
		
		
		// ---- SOUND VOLUME CONTROL -----
		
		private float _stVolume;
		private float _amVolume;
		private float _sfxVolume;

		public void ChangeVolume(AudioCategory category, float value)
		{
			switch (category)
			{
				case AudioCategory.SOUNDTRACK:
					_stVolume = value;
					_soundtrackSrc.volume = value;
					break;
					
				case AudioCategory.AMBIENCE:
					_amVolume = value;
					_ambienceSrc.volume = value;
					break;
				
				case AudioCategory.SFX:
					_sfxVolume = value;
					break;
			}
		}

		#endregion

		#region Private Functions

		private void Configure()
		{
			// Instance = this;
			_audioTable = new Hashtable();
			_jobTable = new Hashtable();
			_playlistTable = new Hashtable();
			PopulateAudioTable();
		}

		/// <summary>
		/// 
		/// </summary>
		private void Dispose()
		{
			if (_jobTable == null) return;
			foreach (DictionaryEntry entry in _jobTable)
			{
				IEnumerator job = (IEnumerator) entry.Value;
				StopCoroutine(job);
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		private void PopulateAudioTable()
		{
			// populate with Sfx
			for (int i = 0; i < audioData.SFXList.Length; i++)
			{
				if (_audioTable.Contains(audioData.SFXList[i].Id))
				{
					LogWarning("Trying to registed Sfx ["+audioData.SFXList[i].Id+"] that has already been registered.");
				}
				else
				{
					_audioTable.Add(audioData.SFXList[i].Id, audioData.SFXList[i]);
					Log("Registed Sfx ["+audioData.SFXList[i].Id+"].");
				}
			}
			// populate with Soundtrack
			for (int i = 0; i < audioData.MusicPlaylists.Length; i++)
			{
				if (_playlistTable.Contains(audioData.MusicPlaylists[i].Id))
				{
					LogWarning("Trying to registed Str Playlist ["+audioData.MusicPlaylists[i].Id+"] that has already been registered.");
				}
				else
				{
					_playlistTable.Add(audioData.MusicPlaylists[i].Id, audioData.MusicPlaylists[i]);
					Log("Registed Str Playlist ["+audioData.MusicPlaylists[i].Id+"].");
				}
				
				for (int j = 0; j < audioData.MusicPlaylists[i].Tracks.Length; j++)
				{
					if (_audioTable.Contains(audioData.MusicPlaylists[i].Tracks[j].Id))
					{
						LogWarning("Trying to registed Soundtrack ["+audioData.MusicPlaylists[i].Tracks[j].Id+"] that has already been registered.");
					}
					else
					{
						_audioTable.Add(audioData.MusicPlaylists[i].Tracks[j].Id, audioData.MusicPlaylists[i].Tracks[j]);
						Log("Registed Soundtrack ["+audioData.MusicPlaylists[i].Tracks[j].Id+"].");
					}
				}
			}
			// populate with Ambiences
			for (int i = 0; i < audioData.AmbientPlaylists.Length; i++)
			{
				if (_playlistTable.Contains(audioData.AmbientPlaylists[i].Id))
				{
					LogWarning("Trying to registed Amb Playlist ["+audioData.AmbientPlaylists[i].Id+"] that has already been registered.");
				}
				else
				{
					_playlistTable.Add(audioData.AmbientPlaylists[i].Id, audioData.AmbientPlaylists[i]);
					Log("Registed Amb Playlist ["+audioData.AmbientPlaylists[i].Id+"].");
				}
				
				for (int j = 0; j < audioData.AmbientPlaylists[i].Tracks.Length; j++)
				{
					if (_audioTable.Contains(audioData.AmbientPlaylists[i].Tracks[j].Id))
					{
						LogWarning("Trying to registed Ambience ["+audioData.AmbientPlaylists[i].Tracks[j].Id+"] that has already been registered.");
					}
					else
					{
						_audioTable.Add(audioData.AmbientPlaylists[i].Tracks[j].Id, audioData.AmbientPlaylists[i].Tracks[j]);
						Log("Registed Ambience ["+audioData.AmbientPlaylists[i].Tracks[j].Id+"].");
					}
				}
			}
		}

		private IEnumerator RunAudioJob(AudioJob job)
		{
			yield return new WaitForSeconds(job.Delay);
			
			// AudioTrack track = (AudioTrack) _audioTable[job.ID];
			
			// si especifica unas coordenadas --> se usa el Pool de Sources
			// si no --> usar un Source en la cámara
			
			// AudioObject obj = GetAudioObjectFromAudioTrack(job.ID, track);
			AudioObject obj = (AudioObject) _audioTable[job.Id];
			// track.Source.clip = obj.Clip;
			job.Source.clip = obj.Clip;

			// float vol = obj.Volume;
			float vol = job.MainVolume;

			switch (job.Action)
			{
				case AudioAction.PLAY:
					// track.Source.volume = obj.Volume;
					// track.Source.Play();
					job.Source.volume = vol;
					job.Source.Play();
					break;
				case AudioAction.STOP:
					if (!job.Fade)
					{
						// track.Source.Stop();
						job.Source.Stop();
					}
					break;
				case AudioAction.RESTART:
					// track.Source.Stop();
					// track.Source.Play();
					job.Source.Stop();
					job.Source.Play();
					break;
			}

			if (job.Fade)
			{
				float inital = job.Action == AudioAction.PLAY || job.Action == AudioAction.RESTART ? 0.0f : vol;
				float target = inital == 0.0f ? vol : 0.0f;
				// float duration = 1.0f;
				float timer = 0.0f;

				while (timer <= _fadeDuration)
				{
					// track.Source.volume = Mathf.Lerp(inital, target, timer / duration);
					job.Source.volume = Mathf.Lerp(inital, target, timer / _fadeDuration);
					timer += Time.deltaTime;
					yield return null;
				}

				if (job.Action == AudioAction.STOP)
				{
					// track.Source.Stop();
					job.Source.Stop();
				}
			}
			
			_jobTable.Remove(job.Id);
			Log("Job count: "+_jobTable.Count);
			
			while (job.Source.isPlaying)
			{
				yield return null;
			}
			_sourcesAtCoordsPool.GiveBackAudioSource(job.Source);

			yield return null;
		}

		private IEnumerator RunPlaylistJob(PlaylistJob job)
		{
			yield return new WaitForSeconds(job.InitialDelay);
			
			// AudioObject obj = GetAudioObjectFromAudioTrack(job.ID, track);
			AudioPlaylist playlist = (AudioPlaylist) _playlistTable[job.Id];

			switch (job.Action)
			{
				case AudioAction.PLAY:
					bool continueLoop = true;
					while (continueLoop)
					{
						for (int i = 0; i < playlist.Tracks.Length; i++)
						{
							job.Source.clip = playlist.Tracks[i].Clip;
							job.Source.volume = job.MainVolume * playlist.Tracks[i].Volume;
							job.Source.Play();

							while (job.Source.isPlaying)
							{
								yield return null;
							}
							
							// ¿ fade between tracks ?
						}
						continueLoop = playlist.Loops;
					}
					break;
				
				case AudioAction.STOP:
					
					// job.Source.Stop();
					
					if (!job.Fade)
					{
						// RemovePlaylistJob(job.Id);
						job.Source.Stop();
					}
					break;
				
				case AudioAction.RESTART:
					job.Source.Stop();
					job.Source.Play();
					break;
			}

			if (job.Fade)
			{
				float vol = job.Source.volume; // todo 
				
				float inital = job.Action == AudioAction.PLAY || job.Action == AudioAction.RESTART ? 0.0f : vol;
				float target = inital == 0.0f ? vol : 0.0f;
				// float duration = 1.0f;
				float timer = 0.0f;
			
				while (timer <= _fadeDuration)
				{
					// track.Source.volume = Mathf.Lerp(inital, target, timer / duration);
					job.Source.volume = Mathf.Lerp(inital, target, timer / _fadeDuration);
					timer += Time.deltaTime;
					yield return null;
				}
			
				if (job.Action == AudioAction.STOP)
				{
					// track.Source.Stop();
					// RemovePlaylistJob(job.Id);
					job.Source.Stop();
				}
			}
			
			_jobTable.Remove(job.Id);
			Log("Job count: "+_jobTable.Count);

			yield return null;
		}
		
		private void AddPlaylistJob(PlaylistJob job)
		{
			RemoveConflictingPlaylistJobs(job.Id);
			// start job
			IEnumerator jobRunner = RunPlaylistJob(job);
			if (!_jobTable.Contains(job.Id)) _jobTable.Add(job.Id, jobRunner);
			StartCoroutine(jobRunner);
			Log("Starting job on ["+job.Id+"] with operation: " + job.Action);
		}

		private void AddJob(AudioJob job)
		{
			// RemoveConflictingJobs(job.ID);
			// start job
			IEnumerator jobRunner = RunAudioJob(job);
			if (!_jobTable.Contains(job.Id)) _jobTable.Add(job.Id, jobRunner);
			StartCoroutine(jobRunner);
			Log("Starting job on ["+job.Id+"] with operation: " + job.Action);
		}

		private void RemoveJob(AudioId id)
		{
			if (_jobTable.Contains(id))
			{
				LogWarning("Trying to stop a job ["+id+"] that is not running.");
				return;
			}

			IEnumerator runningJob = (IEnumerator) _jobTable[id];
			StopCoroutine(runningJob);
			_jobTable.Remove(id);
		}
		
		private void RemovePlaylistJob(PlaylistId id)
		{
			// if (_jobTable.Contains(id))
			// {
			// 	LogWarning("Trying to stop a job ["+id+"] that is not running.");
			// 	return;
			// }

			IEnumerator runningJob = (IEnumerator) _jobTable[id];
			StopCoroutine(runningJob);
			_jobTable.Remove(id);
		}
		
		private void RemoveConflictingPlaylistJobs(PlaylistId id)
		{
			if (_jobTable.Contains(id))
			{
				RemovePlaylistJob(id);
			}
		}

		private void Log(string msg)
		{
			if (!_debug) return;
			UnityEngine.Debug.Log("[Audio Controller]: "+msg);
		}
		
		private void LogWarning(string msg)
		{
			if (!_debug) return;
			UnityEngine.Debug.LogWarning("[Audio Controller]: "+msg);
		}

		#endregion
	}
}