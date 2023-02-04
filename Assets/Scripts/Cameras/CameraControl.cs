using Gameplay;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Cameras
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Transform _swivel;
        [SerializeField] private Transform _stick;
        
        [SerializeField] private CameraData cameraData;
        
        [SerializeField] private PostProcessVolume _volume;
        public PostProcessVolume PPVolume => _volume;
        public DepthOfField DepthOfField;

        private float _zoom;
        private CameraData.StickZoom _stickZoom;
        private CameraData.SwivelZoom _swivelZoom;
        private float rotationAngle;

        private float _gridSize;

        private Rect _gridBounds;

        public void Init()
        {
            _zoom = 0.0f;
            
            _stickZoom = new CameraData.StickZoom
            {
                MinZoom = cameraData.StickZoomCam.MinZoom,
                MaxZoom = cameraData.StickZoomCam.MaxZoom
            };
            _swivelZoom = new CameraData.SwivelZoom
            {
                MinZoom = cameraData.SwivelZoomCam.MinZoom,
                MaxZoom = cameraData.SwivelZoomCam.MaxZoom
            };

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            _stick.localPosition = new Vector3(0, 0, -20);
            
            PPVolume.profile.TryGetSettings(out DepthOfField); 
        }

        private void Update()
        {
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0f)
            {
                AdjustZoom(zoomDelta);
            }

            float rotationDelta = Input.GetAxis("Rotation");
            
            /*if (Input.GetMouseButton(1))
            {
                rotationDelta = Input.GetAxis("Mouse X");
            }*/
            
            if (rotationDelta != 0f)
            {
                AdjustRotation(rotationDelta);
            }

            float xDelta = Input.GetAxis("Horizontal");
            float zDelta = Input.GetAxis("Vertical");
            
            /*if (Input.GetMouseButton(0))
            {
                xDelta = Input.GetAxis("Mouse X") * -1f * 0.5f;
                zDelta = Input.GetAxis("Mouse Y") * -1f * 0.5f;
            }*/
            
            if (xDelta != 0f || zDelta != 0f)
            {
                AdjustPosition(xDelta, zDelta);
            }
            
            // ----------------------------------------------
        }
        
        public void UpdateGridBounds(Rect newBounds)
        {
            _gridBounds = newBounds;
        }

        private void AdjustRotation(float delta)
        {
            rotationAngle += delta * cameraData.RotationSpeed * Time.deltaTime;
            if (rotationAngle < 0f)
            {
                rotationAngle += 360f;
            } else if (rotationAngle >= 360f)
            {
                rotationAngle -= 360f;
            }
            transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }

        private void AdjustPosition(float xDelta, float zDelta)
        {
            Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
            float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
            float distance = 
                Mathf.Lerp(cameraData.MoveSpeedZoomCam.MinZoom, cameraData.MoveSpeedZoomCam.MaxZoom, _zoom) *
                damping * Time.deltaTime;
            
            Vector3 position = transform.localPosition;
            position += direction * distance;
            transform.localPosition = ClampPosition(position);
        }

        /// <summary>
        /// Ensures the updated position of the camera doesn't surpass the grid bounds.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector3 ClampPosition(Vector3 position)
        {
            float xMax = _gridBounds.xMax + cameraData.Margins;
            float xMin = _gridBounds.xMin - cameraData.Margins;
            position.x = Mathf.Clamp(position.x, xMin, xMax);
            float zMax = _gridBounds.yMax + cameraData.Margins;
            float zMin = _gridBounds.yMin - cameraData.Margins;
            position.z = Mathf.Clamp(position.z, zMin, zMax);
            
            return position;
        }

        private void AdjustZoom(float delta)
        {
            _zoom = Mathf.Clamp01(_zoom + delta);

            float distance = Mathf.Lerp(_stickZoom.MinZoom, _stickZoom.MaxZoom, _zoom);
            _stick.localPosition = new Vector3(0f, 0f, distance);

            float angle = Mathf.Lerp(_swivelZoom.MinZoom, _swivelZoom.MaxZoom, _zoom);
            _swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }

        // private void RepositionCamera()
        // {
        //     transform.position = Vector3.zero;
        //     transform.rotation = Quaternion.identity;
        //     _swivel.rotation = new Quaternion(45, 0, 0, 1);
        //     _stick.position = new Vector3(0, 0, -20);
        // }
    } 
}

