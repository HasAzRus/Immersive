using System;
using Oxygen;
using UnityEngine;

namespace Iron
{
    public enum PlayerMode
    {
        None,
        Inventory,
    }
    
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(PlayerBob))]
    public class IronPlayer : FirstPersonPlayer
    {
        public event Action<float> GrabbingTimeValueChanged; 

        public event Action InventoryOpened;
        public event Action InventoryClosed;

        public event Action<float> StaminaChanged; 

        [SerializeField] private float _maxStamina;
        [SerializeField] private float _minEnoughStamina;

        [SerializeField] private float _runningStaminaCost;
        [SerializeField] private float _jumpStaminaCost;
        
        [SerializeField] private float _staminaRest;

        [SerializeField] private float _deadlyHeight;
        [SerializeField] private float _fallenDamageAmount;

        [SerializeField] private float _stopInteractingDistance;
        
        private bool _isTimingToInteract;
        
        private float _maxTimeToInteract;
        private float _timeToInteract;
        
        private BaseTimeGrabInteractive _timeGrabInteractive;

        private bool _isStaminaEnough;
        private float _stamina;
        
        private PlayerMode _mode;
        
        private IronPlayerInventory _inventory;
        private IronPlayerCamera _ironCamera;
        private PlayerBob _bob;

        private Transform _transform;

        private void OnModeChanged(PlayerMode value)
        {
            GetInput().SetMode(value == PlayerMode.Inventory ? InputMode.UI : InputMode.Game);
        }
        
        private void OnUncrouched()
        {
            _ironCamera.UpdateHeightCrouch(false);
        }

        private void OnCrouched()
        {
            _ironCamera.UpdateHeightCrouch(true);
        }
        
        private void SetMode(PlayerMode value)
        {
            _mode = value;
            
            OnModeChanged(value);
        }

        private void SetStamina(float value)
        {
            _stamina = value;
            
            StaminaChanged?.Invoke(value);
        }
        
        private void OnJumping()
        {
            SpendStamina(_jumpStaminaCost);
        }
        
        private void OnFallen(float height)
        {
            if (height < _deadlyHeight)
            {
                return;
            }
            
            ApplyDamage(gameObject, (height - _deadlyHeight) * _fallenDamageAmount);
        }
        
        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();

            _inventory = GetComponent<IronPlayerInventory>();
            _bob = GetComponent<PlayerBob>();

            _transform = transform;
            
            _inventory.Construct(this);
            
            if (GetMotor() is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }
            
            ironPlayerMotor.Crouched += OnCrouched;
            ironPlayerMotor.Uncrouched += OnUncrouched;
            
            ironPlayerMotor.Jumping += OnJumping;
            
            ironPlayerMotor.Fallen += OnFallen;
            
            _ironCamera = GetCamera() as IronPlayerCamera;
        }

        protected override void OnGameBeginned()
        {
            base.OnGameBeginned();
            
            SetMode(PlayerMode.None);
        }

        protected override void OnDamageApplied(GameObject caller, float damage)
        {
            base.OnDamageApplied(caller, damage);

            var direction = (caller.transform.position - _transform.position).normalized;
            direction.y = 0f;

            if (GetMotor() is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }
            
            ironPlayerMotor.AddForce(-direction * damage);
        }

        protected override void OnInteracting()
        {
            base.OnInteracting();
            
            if (GetInteraction().CurrentInteractive is not BaseTimeGrabInteractive timeGrabInteractive)
            {
                return;
            }

            _isTimingToInteract = true;

            _timeToInteract = 0f;
            _maxTimeToInteract = timeGrabInteractive.GetMaxTime();

            _timeGrabInteractive = timeGrabInteractive;
        }

        protected override void OnInteractionStopped()
        {
            base.OnInteractionStopped();
            
            if (_isTimingToInteract)
            {
                _timeToInteract = 0f;
                _isTimingToInteract = false;
            }

            _timeGrabInteractive = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (GetMotor() is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }
            
            ironPlayerMotor.Crouched -= OnCrouched;
            ironPlayerMotor.Uncrouched -= OnUncrouched;

            ironPlayerMotor.Jumping -= OnJumping;
            
            ironPlayerMotor.Fallen -= OnFallen;
        }

        protected override void Update()
        {
            base.Update();

            var deltaTime = Time.deltaTime;

            if (_isTimingToInteract)
            {
                if (_timeToInteract < _maxTimeToInteract)
                {
                    _timeToInteract += deltaTime;
                }
                else
                {
                    _timeGrabInteractive.StopTimeInteraction();

                    TryStopInteraction();

                    _timeToInteract = _maxTimeToInteract;
                    _isTimingToInteract = false;
                }
                
                GrabbingTimeValueChanged?.Invoke(_timeToInteract);
            }

            if (GetInteraction().IsInteracting)
            {
                var currentInteractive = GetInteraction().CurrentInteractive;

                if (currentInteractive is BaseGrabInteractive grabInteractive)
                {
                    var grabInteractiveTransform = grabInteractive.transform;
                    var distance = Vector3.Distance(_transform.position, grabInteractiveTransform.position);

                    if (distance > _stopInteractingDistance)
                    {
                        TryStopInteraction();
                    }
                }
            }
            
            if (GetMotor() is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }

            if (ironPlayerMotor.GetVelocity2d().magnitude > 0.02f)
            {
                _bob.Bob(ironPlayerMotor.GetDirection());
            }
            else
            {
                if (_bob.CheckIsBobing())
                {
                    _bob.StopBobing();
                }
            }

            if (ironPlayerMotor.CheckRunning())
            {
                SpendStamina(_runningStaminaCost * deltaTime);
            }
            else
            {
                AddStamina(_staminaRest * deltaTime);
            }
        }

        public IronPlayerInventory GetInventory()
        {
            return _inventory;
        }

        public void OpenInventory()
        {
            SetMode(PlayerMode.Inventory);
            
            InventoryOpened?.Invoke();
        }

        public void CloseInventory()
        {
            SetMode(PlayerMode.None);
            
            InventoryClosed?.Invoke();
        }

        public PlayerMode GetMode()
        {
            return _mode;
        }

        public void AddStamina(float amount)
        {
            if (_stamina + amount < _maxStamina)
            {
                SetStamina(_stamina + amount);

                if (_stamina > _minEnoughStamina)
                {
                    _isStaminaEnough = true;
                }
            }
            else
            {
                SetStamina(_maxStamina);
            }
        }

        public void SpendStamina(float amount)
        {
            if (_stamina - amount > 0f)
            {
                SetStamina(_stamina - amount);
            }
            else
            {
                _isStaminaEnough = false;
                
                SetStamina(0f);
            }
        }

        public bool CheckStaminaEnough()
        {
            return _isStaminaEnough;
        }

        public float GetMaxStamina()
        {
            return _maxStamina;
        }

        public BaseTimeGrabInteractive GetTimeGrabInteractive()
        {
            return _timeGrabInteractive;
        }
    }
}