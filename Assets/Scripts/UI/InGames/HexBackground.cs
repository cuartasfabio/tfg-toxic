using Cameras;
using Gameplay;
using UnityEngine;

namespace UI.InGames
{
	public class HexBackground : MonoBehaviour
	{
		private RaycastManager _rm;
		
		private void Start()
		{
			_rm = ObjectCache.Current.RaycastManager;
		}

		private void LateUpdate()
		{
			Vector3 screenCentre = _rm.GetGridPointFromScreenCentre();
			transform.position = new Vector3(screenCentre.x, -0.001f, screenCentre.z);
		}
	}
}