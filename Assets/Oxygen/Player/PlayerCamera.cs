using UnityEngine;

namespace Oxygen
{
    public class PlayerCamera : Behaviour
    {
        [SerializeField] private Camera _camera;

        private Camera _toggledCamera;
        
        private Transform _cameraTransform;
        private Transform _transform;

        protected virtual void OnClear()
        {
            
        }

        protected override void Start()
        {
            base.Start();
            
            _transform = transform;
            _cameraTransform = _camera.transform;
        }

        public void ToggleTo(Camera camera)
        {
            _camera.enabled = false;

            camera.enabled = true;
            _toggledCamera = camera;
        }

        public void ToggleToDefault()
        {
            _toggledCamera.enabled = false;
            _toggledCamera = null;

            _camera.enabled = true;
        }

        public void Clear()
        {
            OnClear();
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public Transform GetCameraTransform()
        {
            return _cameraTransform;
        }

        public Transform GetTransform()
        {
            return _transform;
        }
    }
}