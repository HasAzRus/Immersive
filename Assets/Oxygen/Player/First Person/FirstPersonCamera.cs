using UnityEngine;

namespace Oxygen
{
    public class FirstPersonCamera : PlayerCamera
    {
        [SerializeField] private float _minVerticalAngle;
        [SerializeField] private float _maxVerticalAngle;

        private Coroutine _smoothFieldOfViewRoutine;
        
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

            _initialCameraRotation = CameraTransform.localRotation;
            _initialTransformRotation = Transform.rotation;

            DefaultFieldOfView = Camera.fieldOfView;
        }

        protected override void Update()
        {
            base.Update();

            var deltaTime = Time.deltaTime;

            _verticalForce = Mathf.Lerp(_verticalForce, _inputVerticalForce, deltaTime * 4);
            _horizontalForce = Mathf.Lerp(_horizontalForce, _inputHorizontalForce, deltaTime * 4);
            
            if (_isMouseLookEnabled)
            {
                CameraTransform.localRotation =
                    _initialCameraRotation * Quaternion.AngleAxis(_verticalAngle + _verticalForce, Vector3.right) *
                    Quaternion.AngleAxis(_horizontalForce, Vector3.up) *
                    Quaternion.AngleAxis(_horizontalForce * 0.8f, Vector3.forward);
                
                Transform.rotation =
                    _initialTransformRotation * Quaternion.AngleAxis(_horizontalAngle, Vector3.up);
            }

            _inputHorizontalForce /= 1.4f;
            _inputVerticalForce /= 1.4f;
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
            Camera.fieldOfView = value;
        }

        public void SetSmoothFieldOfView(float value, float duration)
        {
            if (_smoothFieldOfViewRoutine != null)
            {
                Coroutines.Stop(_smoothFieldOfViewRoutine);
            }
            
            _smoothFieldOfViewRoutine = Coroutines.Run(ProcedureAnimation.PlayRoutine(duration, 
                deltaTIme =>
            {
                var fieldOfView = Camera.fieldOfView;
                fieldOfView = Mathf.Lerp(fieldOfView, value, deltaTIme);

                Camera.fieldOfView = fieldOfView;
            }, () =>
            {
                Camera.fieldOfView = value;
            }));
        }
        
        public float DefaultFieldOfView { get; private set; }
    }
}