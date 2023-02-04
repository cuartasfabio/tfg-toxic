using System;
using UnityEngine;

namespace Cameras
{
    [Serializable, CreateAssetMenu(fileName = "New CameraData", menuName = "Cameras/CameraData")]
    public class CameraData: ScriptableObject
    {
        [Serializable]
        public class StickZoom
        {
            public float MinZoom = -8;
            public float MaxZoom = -3;
        }
        
        [Serializable]
        public class SwivelZoom
        {
            public float MinZoom = 50;
            public float MaxZoom = 30;
        }

        [Serializable]
        public class MoveSpeedZoom
        {
            public float MinZoom = 15;
            public float MaxZoom = 10;
        }

        [SerializeField] public float RotationSpeed = 75;
        
        [SerializeField] public StickZoom StickZoomCam;
        
        [SerializeField] public SwivelZoom SwivelZoomCam;
        
        [SerializeField] public MoveSpeedZoom MoveSpeedZoomCam;

        /// <summary>
        /// Offset used to clamp the position of the camera depending on the size of the grid.
        /// </summary>
        [SerializeField, Range(0.0f, 3.0f)] public float Margins = 1.5f;

    }
}