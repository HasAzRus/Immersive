using UnityEngine;

namespace Oxygen
{
    public class PlayerCamera : Behaviour
    {
        [SerializeField] private Camera _camera;

        private Camera _toggledCamera;

        protected virtual void OnClear()
        {
            
        }

        protected override void Start()
        {
            base.Start();
            
            Transform = transform;
            CameraTransform = _camera.transform;
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

        public Camera Camera => _camera;
        public Transform CameraTransform { get; private set; }
        public Transform Transform { get; private set; }
    }
}