using UnityEngine;

namespace UI
{
	public class ParallaxMouse : MonoBehaviour
	{
		[SerializeField] private float _strength = 0;
		[SerializeField] private Vector2 _clamp = new Vector2(75,75);

		private Vector2 _startPos;
		private Camera _camera;

		private void Start()
		{
			_camera = Camera.main;
			_startPos = transform.position;
		}

		private void Update()
		{
			Vector2 mousePos = _camera.ScreenToViewportPoint(Input.mousePosition);
			Vector3 currentPos = transform.position;

			// lerp between position and mousePos
			float posX = Mathf.Lerp(currentPos.x, _startPos.x + (-mousePos.x * _strength), 2f * Time.deltaTime);
			float posY = Mathf.Lerp(currentPos.y, _startPos.y + (-mousePos.y * _strength), 2f * Time.deltaTime);
			
			posX = Mathf.Clamp(posX, _startPos.x - _clamp.x,_startPos.x + _clamp.x);
			posY = Mathf.Clamp(posY, _startPos.y - _clamp.y,_startPos.y + _clamp.y);
			
			transform.position = new Vector3(posX, posY, currentPos.z);
		}
	}
}