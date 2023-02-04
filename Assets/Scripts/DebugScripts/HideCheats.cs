using UnityEngine;

namespace DebugScripts
{
	public class HideCheats : MonoBehaviour
	{
		[SerializeField] private CommandConsole _commandConsole;

		private void Start()
		{
			_commandConsole.gameObject.SetActive(false);
			
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Backslash))
			{
				_commandConsole.gameObject.SetActive(!_commandConsole.gameObject.activeSelf);
				_commandConsole.Init();
			}
				
		}
	}
}