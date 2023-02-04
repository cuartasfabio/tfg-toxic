using Controls;
using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cameras
{
    public class RaycastManager: MonoBehaviour
    {
        public Vector3 LastHitOnGrid { get; private set; }
        
        private Camera _mainCamera;

        private Plane _gridPlane;
        
        
        public void Init()
        {
            LastHitOnGrid = new Vector3(100,0,100);
            _gridPlane = new Plane(Vector3.up, Vector3.zero);
        }
        
        private void Start()
        {
            _mainCamera = ObjectCache.Current.MainCamera;
        }

        public bool GetRaycastOnGrid()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;
            
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            
            float enter;

            if (_gridPlane.Raycast(ray, out enter) && GameControls.AreControlsEnabled())
            {
                LastHitOnGrid = ray.GetPoint(enter);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gives the point on the GridPlane at the center og the screen.
        /// </summary>
        /// <returns>The point at the centre of the screen.</returns>
        public Vector3 GetGridPointFromScreenCentre()
        {
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width/2f,Screen.height/2f,0f));
            
            float enter;

            if (_gridPlane.Raycast(ray, out enter))
            {
                return ray.GetPoint(enter);
            }
            return _mainCamera.transform.position;
        }
    }
}