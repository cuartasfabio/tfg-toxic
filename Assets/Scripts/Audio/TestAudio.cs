using UnityEngine;

namespace Audio
{
	public class TestAudio : MonoBehaviour
	{
		
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.T))
			{
				AudioController.Get().PlaySfx(AudioId.ST_MainMenu, true, 1.0f);
			}
			if (Input.GetKeyUp(KeyCode.G))
			{
				AudioController.Get().StopSfx(AudioId.ST_MainMenu, true);
			}
			if (Input.GetKeyUp(KeyCode.B))
			{
				AudioController.Get().RestartSfx(AudioId.ST_MainMenu, true);
			}
			
			
			if (Input.GetKeyUp(KeyCode.Y))
			{
				AudioController.Get().PlaySfx(AudioId.SFX_PageForward);
			}
			if (Input.GetKeyUp(KeyCode.H))
			{
				AudioController.Get().StopSfx(AudioId.SFX_PageForward);
			}
			if (Input.GetKeyUp(KeyCode.N))
			{
				AudioController.Get().RestartSfx(AudioId.SFX_PageForward);
			}
		}
	}
}