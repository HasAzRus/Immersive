using System;
using Oxygen;
using UnityEngine;

namespace Iron
{
    public class IronPlayerCamera : FirstPersonCamera
    {
        [SerializeField] private Transform _headTransform;
        
        [SerializeField] private float _normalHeight;
        [SerializeField] private float _crouchHeight;

        [SerializeField] private float _crouchSpeed;

        private float _targetHeight;
        
        private bool _isCameraUpdatingHeight;

        protected override void Update()
        {
            base.Update();
            
            if (_isCameraUpdatingHeight)
            {
                var position = _headTransform.localPosition;
                position.y = Mathf.Lerp(position.y, _targetHeight, Time.deltaTime * _crouchSpeed);
                
                if (Math.Abs(position.y - _targetHeight) < 0.02f)
                {
                    position.y = _targetHeight;

                    _isCameraUpdatingHeight = false;
                }

                _headTransform.localPosition = position;
            }
        }

        public void UpdateHeightCrouch(bool crouchState)
        {
            _isCameraUpdatingHeight = true;

            _targetHeight = crouchState ? _crouchHeight : _normalHeight;
        }
    }
}