using UnityEngine;

namespace Oxygen
{
    public class FirstPersonCamera : PlayerCamera
    {
        [SerializeField] private float _minVerticalAngle;
        [SerializeField] private float _maxVerticalAngle;
        
        private float _defaultFieldOfView;

        private bool _isMouseLookEnabled;

        private float _verticalForce;
        private float _horizontalForce;

        private float _inputVerticalForce;
        private float _inputHorizontalForce;
        
        private float _verticalAngle;
        private float _horizontalAngle;
        
        private Quaternion _initialCameraRotation;
        private Quaternion _initialTransformRotation;

        protected override void OnClear()
        {
            base.OnClear();

            _verticalAngle = 0;
            _horizontalAngle = 0;
        }

        protected override void Start()
        {
            base.Start();

            _initialCameraRotation = GetCameraTransform().localRotation;
            _initialTransformRotation = GetTransform().rotation;

            _defaultFieldOfView = GetCamera().fieldOfView;
        }

        protected override void Update()
        {
            base.Update();

            var deltaTime = Time.deltaTime;

            _verticalForce = Mathf.Lerp(_verticalForce, _inputVerticalForce, deltaTime * 4);
            _horizontalForce = Mathf.Lerp(_horizontalForce, _inputHorizontalForce, deltaTime * 4);
            
            if (_isMouseLookEnabled)
            {
                GetCameraTransform().localRotation =
                    _initialCameraRotation * Quaternion.AngleAxis(_verticalAngle + _verticalForce, Vector3.right) *
                    Quaternion.AngleAxis(_horizontalForce, Vector3.up);
                
                GetTransform().rotation =
                    _initialTransformRotation * Quaternion.AngleAxis(_horizontalAngle, Vector3.up);
            }

            _inputHorizontalForce *= 0.98f;
            _inputVerticalForce *= 0.98f;
        }

        public void LookAt(float value)
        {
            if (!_isMouseLookEnabled)
            {
                return;
            }
            
            _verticalAngle += value;

            _verticalAngle = Mathf.Clamp(_verticalAngle, _minVerticalAngle, _maxVerticalAngle);
        }

        public void Turn(float value)
        {
            if (!_isMouseLookEnabled)
            {
                return;
            }
            
            _horizontalAngle += value;
        }

        public void AddHorizontalForce(float value)
        {
            _horizontalForce = value;
        }
        
        public void AddVerticalForce(float value)
        {
            _verticalForce = value;
        }

        public void SetMouseLookEnabled(bool value)
        {
            _isMouseLookEnabled = value;
        }

        public void SetFieldOfView(float value)
        {
            GetCamera().fieldOfView = value;
        }

        public void ClearFieldOfView()
        {
            SetFieldOfView(_defaultFieldOfView);
        }
    }
}