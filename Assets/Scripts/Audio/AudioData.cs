using System;
using UnityEngine;

namespace Audio
{
	[Serializable, CreateAssetMenu(fileName = "New AudioData", menuName = "Audio/AudioData")]
	
	public class AudioData : ScriptableObject
	{
		/// <summary>
		/// Stores the music playlists for every level/screen
		/// </summary>
		[SerializeField] public AudioController.AudioPlaylist[] MusicPlaylists;
		/// <summary>
		/// Stores the ambience playlists for every level/screen
		/// </summary>
		[Space(10)]
		[SerializeField] public AudioController.AudioPlaylist[] AmbientPlaylists;
		/// <summary>
		/// List of every sound effect
		/// </summary>
		[Space(10)]
		[SerializeField] public AudioController.AudioObject[] SFXList;
	}
}