using System;
using Oxygen;
using UnityEngine;
using UnityEngine.UI;

namespace Iron
{
    public class IronHeadUpDisplay : HeadUpDisplay
    {
        [SerializeField] private IronHealthWidget _healthWidget;
        [SerializeField] private IronStaminaWidget _staminaWidget;
        [SerializeField] private IronEnergyWidget _energyWidget;
        [SerializeField] private IronWeaponWidget _weaponWidget;

        [SerializeField] private Image _timeGrabbingCrosshairImage;

        [SerializeField] private GameObject _crosshairGameObject;

        private bool _isGrabbing;
        private bool _isCrosshairActive;
        
        private bool _isPointing;

        private Transform _pointerTargetTransform;
        private Transform _crosshairTransform;

        private Camera _camera;

        private IronPlayer _ironPlayer;
        
        protected override void OnConstruction(Player player)
        {
            base.OnConstruction(player);

            if (player is not IronPlayer ironPlayer)
            {
                return;
            }
            
            _healthWidget.Construct(ironPlayer);
            _staminaWidget.Construct(ironPlayer);
            _energyWidget.Construct(ironPlayer);
            _weaponWidget.Construct(ironPlayer);
            
            ironPlayer.Interacting += OnInteracting;
            ironPlayer.InteractionStopped += OnInteractionStopped;
            ironPlayer.GrabbingTimeValueChanged += OnGrabbingTimeValueChanged;

            var pointer = ironPlayer.Pointer;
            
            pointer.Pointed += OnPointed;
            pointer.Unpointed += OnUnpointed;

            _camera = ironPlayer.Camera.Camera;
            
            _ironPlayer = ironPlayer;
        }

        protected override void Start()
        {
            base.Start();

            _crosshairTransform = _crosshairGameObject.transform;
            
            _energyWidget.Hide();
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
                MathF.Round(value / _ironPlayer.TimeGrabInteractive.GetMaxTime(), 2);
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
            
            _healthWidget.Clear();
            _staminaWidget.Clear();
            _energyWidget.Clear();
            _weaponWidget.Clear();

            _ironPlayer.Interacting += OnInteracting;
            _ironPlayer.InteractionStopped += OnInteractionStopped;
            _ironPlayer.GrabbingTimeValueChanged += OnGrabbingTimeValueChanged;
            
            var pointer = _ironPlayer.Pointer;
            
            pointer.Pointed -= OnPointed;
            pointer.Unpointed -= OnUnpointed;

            _camera = null;
        }
    }
}