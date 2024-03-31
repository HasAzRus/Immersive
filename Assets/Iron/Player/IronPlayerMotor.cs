using System;
using Oxygen;
using UnityEngine;

namespace Iron
{
    public class IronPlayerMotor : FirstPersonMotor
    {
        public event Action<float> Fallen; 

        public event Action Jumping;
        
        public event Action Crouched;
        public event Action Uncrouched;
        
        [SerializeField] private float _jumpHeight;
        
        [SerializeField] private float _normalHeight;
        [SerializeField] private float _crouchHeight;

        [SerializeField] private float _checkCeilingDistance;
        [SerializeField] private LayerMask _checkCeilingLayerMask;

        [SerializeField] private float _crouchSpeedAmount;
        [SerializeField] private float _runningSpeedAmount;
        [SerializeField] private float _aimingSpeedAmount;

        private float _smoothRunningSpeedAmount;

        private Vector3 _startFallingPosition;

        private bool _isFalling;

        private Vector3 _forceDirection;

        private bool _isCrouching;
        private bool _isRunning;
        
        private bool _isJumpInput;
        private bool _isJumping;

        private bool _isAiming;
        
        private bool CheckCeiling()
        {
            var result = Physics.Raycast(GetTransform().position, Vector3.up, _checkCeilingDistance,
                _checkCeilingLayerMask);

            return result;
        }

        private void ShouldCrouch(bool value)
        {
            var characterController = GetCharacterController();

            characterController.center = Vector3.up * (value ? -0.5f : 0f);
            characterController.height = value ? _crouchHeight : _normalHeight;

            if (value)
            {
                Crouched?.Invoke();
            }
            else
            {
                Uncrouched?.Invoke();
            }

            _isCrouching = value;
        }
        
        protected override Vector3 CalculateAdditionalMoveDirection(Vector3 moveDirection)
        {
            if (_isJumpInput)
            {
                moveDirection.y = _jumpHeight;
            }

            return moveDirection + _forceDirection;
        }

        protected override float CalculateSpeedMultiplier()
        {
            var speed = base.CalculateSpeedMultiplier();
            
            if (_isRunning)
            {
                _smoothRunningSpeedAmount =
                    Mathf.Lerp(_smoothRunningSpeedAmount, _runningSpeedAmount, Time.deltaTime * 2f);
                
                speed *= _smoothRunningSpeedAmount;
            }
            
            if (_isCrouching)
            {
                speed *= _crouchSpeedAmount;
            }

            if (_isAiming)
            {
                speed *= _aimingSpeedAmount;
            }

            return speed;
        }

        protected override void Start()
        {
            base.Start();
            
            _smoothRunningSpeedAmount = 1f;
        }

        protected override void Update()
        {
            base.Update();

            var cachedTransform = GetTransform();
            
            if (GetCharacterController().isGrounded)
            {
                if (_isJumping)
                {
                    _isJumping = false;
                }

                if (_isFalling)
                {
                    var height = Mathf.Abs(_startFallingPosition.y - cachedTransform.position.y);
                    
                    Fallen?.Invoke(height);

                    _isFalling = false;
                }
            }
            else
            {
                if (!_isFalling)
                {
                    var velocity = GetCharacterController().velocity;
                    
                    if (velocity.y < 0f)
                    {
                        _startFallingPosition = cachedTransform.position;
                        
                        _isFalling = true;
                    }
                }
            }

            if (_isJumpInput)
            {
                _isJumpInput = false;
            }

            _forceDirection *= 0.98f;
        }

        public void Jump()
        {
            if (_isJumping)
            {
                return;
            }
            
            _isJumpInput = true;
            _isJumping = true;
            
            Jumping?.Invoke();
        }

        public bool CheckCrouching()
        {
            return _isCrouching;
        }

        public void Crouch()
        {
            ShouldCrouch(true);
        }

        public void Uncrouch()
        {
            if (CheckCeiling())
            {
                Debug.Log("Не могу подняться, есть потолок");
                
                return;
            }
            
            ShouldCrouch(false);
        }

        public void StartRunning()
        {
            _isRunning = true;
        }

        public void StopRunning()
        {
            _isRunning = false;

            _smoothRunningSpeedAmount = 1f;
        }

        public void EnableAimingSpeed()
        {
            _isAiming = true;
        }

        public void DisableAimingSpeed()
        {
            _isAiming = false;
        }

        public bool CheckRunning()
        {
            return _isRunning;
        }

        public void AddForce(Vector3 direction)
        {
            _forceDirection += direction;
        }
    }
}