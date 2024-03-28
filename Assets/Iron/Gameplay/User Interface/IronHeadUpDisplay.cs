using System;
using Oxygen;
using UnityEngine;
using UnityEngine.UI;

namespace Iron
{
    public class IronHeadUpDisplay : HeadUpDisplay
    {
        [SerializeField] private float _healthMinValue;
        [SerializeField] private float _staminaMinValue;
        
        [SerializeField] private Image _healthBarImage;
        [SerializeField] private Image _staminaBarImage;

        [SerializeField] private Image _timeGrabbingCrosshairImage;

        [SerializeField] private GameObject _crosshairGameObject;

        private bool _isGrabbing;
        private bool _isCrosshairActive;
        
        private bool _isPointing;

        private Transform _pointerTargetTransform;
        private Transform _crosshairTransform;

        private Camera _camera;

        private IronPlayer _ironPlayer;
        
        protected override void OnConstruct(Player player)
        {
            base.OnConstruct(player);
            
            player.HealthChanged += OnHealthChanged;
            player.DamageApplied += OnDamageApplied;

            if (player is not IronPlayer ironPlayer)
            {
                return;
            }
            
            ironPlayer.StaminaChanged += OnStaminaChanged;
            
            ironPlayer.Interacting += OnInteracting;
            ironPlayer.InteractionStopped += OnInteractionStopped;
            ironPlayer.GrabbingTimeValueChanged += OnGrabbingTimeValueChanged;
            
            var pointer = ironPlayer.GetPointer();
            
            pointer.Pointed += OnPointed;
            pointer.Unpointed += OnUnpointed;

            _camera = ironPlayer.GetCamera().GetCamera();
            
            _ironPlayer = ironPlayer;
        }
        
        protected override void Start()
        {
            base.Start();

            _crosshairTransform = _crosshairGameObject.transform;
        }

        private void OnHealthChanged(float value)
        {
            _healthBarImage.fillAmount = MathF.Round(value / _ironPlayer.MaxHealth, 2);
            _healthBarImage.color = value > _healthMinValue ? Color.white : Color.red;
        }
        
        private void OnStaminaChanged(float value)
        {
            _staminaBarImage.fillAmount = MathF.Round(value / _ironPlayer.MaxStamina, 2);
            _staminaBarImage.color = value > _staminaMinValue ? Color.white : Color.red;
        }
        
        private void OnUnpointed(GameObject target)
        {
            _isPointing = false;

            if (_isGrabbing)
            {
                if (target != null)
                {
                    return;
                }
            }
            
            SetPointerCrosshairActive(null, false);
        }

        private void OnPointed(GameObject target)
        {
            _isPointing = true;
            
            SetPointerCrosshairActive(target.transform, true);
        }
        
        private void OnGrabbingTimeValueChanged(float value)
        {
            if (!_isGrabbing)
            {
                return;
            }

            _timeGrabbingCrosshairImage.fillAmount =
                MathF.Round(value / _ironPlayer.GetTimeGrabInteractive().GetMaxTime(), 2);
        }

        private void OnInteractionStopped()
        {
            _isGrabbing = false;

            if (!_isPointing && _isCrosshairActive)
            {
                SetPointerCrosshairActive(null, false);
            }

            _timeGrabbingCrosshairImage.fillAmount = 0;
            _timeGrabbingCrosshairImage.gameObject.SetActive(false);
        }

        private void OnInteracting()
        {
            _isGrabbing = true;
            
            _timeGrabbingCrosshairImage.gameObject.SetActive(true);
        }
        
        private void OnDamageApplied(GameObject caller, float amount)
        {
            
        }

        private void SetPointerCrosshairActive(Transform target, bool value)
        {
            _isCrosshairActive = value;
            
            _crosshairGameObject.SetActive(value);
            _pointerTargetTransform = target;

            if (!value)
            {
                _crosshairTransform.position = Vector3.zero;
            }
        }
        
        protected override void Update()
        {
            base.Update();

            if (_pointerTargetTransform != null)
            {
                var screenPosition = _camera.WorldToScreenPoint(_pointerTargetTransform.position);

                _crosshairTransform.position = screenPosition;
            }
        }

        protected override void OnClear()
        {
            base.OnClear();

            _ironPlayer.HealthChanged -= OnHealthChanged;
            _ironPlayer.StaminaChanged -= OnStaminaChanged;
            _ironPlayer.DamageApplied -= OnDamageApplied;
            
            _ironPlayer.Interacting += OnInteracting;
            _ironPlayer.InteractionStopped += OnInteractionStopped;
            _ironPlayer.GrabbingTimeValueChanged += OnGrabbingTimeValueChanged;
            
            var pointer = _ironPlayer.GetPointer();
            
            pointer.Pointed -= OnPointed;
            pointer.Unpointed -= OnUnpointed;

            _camera = null;
        }
    }
}