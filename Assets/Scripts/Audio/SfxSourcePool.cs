using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	public class SfxSourcePool : MonoBehaviour
	{
		/// <summary>
		/// AudioSources not in use.
		/// </summary>
		private Queue<AudioSource> _sources;
		
		private void Awake()
		{
			_sources = new Queue<AudioSource>();
		}

		/// <summary>
		/// Returns an available AudioSource (creates one if there are not).
		/// </summary>
		/// <returns>The AudioSource.</returns>
		public AudioSource Request2DAudioSource()
		{
			if (_sources.Count < 1)
			{
				AudioSource source = new GameObject("Sfx source").AddComponent<AudioSource>();
				source.spatialBlend = 0f;
				source.gameObject.transform.parent = transform;
				source.gameObject.SetActive(false);
				_sources.Enqueue(source);
			}
			
			AudioSource outSource = _sources.Dequeue();
			// outSource.spatialBlend = 0f;
			outSource.gameObject.SetActive(true);
			// outSource.Init(tile, fontSize);

			return outSource;
		}

		/// <summary>
		/// Returns a given AudioSource when the job ends.
		/// </summary>
		/// <param name="source">The AudioSource to return.</param>
		public void GiveBackAudioSource(AudioSource source)
		{
			source.clip = null;
			source.gameObject.SetActive(false);
			_sources.Enqueue(source);
		}	
	}
}