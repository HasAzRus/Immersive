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
    
    [RequireComponent(typeof(IronPlayerInventory))]
    [RequireComponent(typeof(PlayerBob))]
    [RequireComponent(typeof(WeaponManager))]
    public class IronPlayer : FirstPersonPlayer, IIronItemCollector
    {
        public event Action<float> GrabbingTimeValueChanged; 

        public event Action InventoryOpened;
        public event Action InventoryClosed;

        [SerializeField] private Stamina _stamina;

        [SerializeField] private float _runningStaminaCost;
        [SerializeField] private float _jumpStaminaCost;
        
        [SerializeField] private float _staminaRest;

        [SerializeField] private float _deadlyHeight;
        [SerializeField] private float _fallenDamageAmount;

        [SerializeField] private float _stopInteractingDistance;
        
        private bool _isTimingToInteract;
        
        private float _maxTimeToInteract;
        private float _timeToInteract;

        private IronPlayerInventory _inventory;
        
        private IronPlayerCamera _ironCamera;
        private PlayerBob _bob;

        private Transform _transform;

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();

            _transform = transform;
            _bob = GetComponent<PlayerBob>();

            _inventory = GetComponent<IronPlayerInventory>();
            _inventory.Construct(this);
            
            if (Motor is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }
            
            ironPlayerMotor.Crouched += OnCrouched;
            ironPlayerMotor.Uncrouched += OnUncrouched;
            
            ironPlayerMotor.Jumping += OnJumping;
            
            ironPlayerMotor.Fallen += OnFallen;
            
            _ironCamera = Camera as IronPlayerCamera;
        }

        protected override void OnGameBeginned()
        {
            base.OnGameBeginned();
            
            SetMode(PlayerMode.None);
        }
        
        private void OnModeChanged(PlayerMode value)
        {
            Input.SetMode(value == PlayerMode.Inventory ? InputMode.UI : InputMode.Game);
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
            Mode = value;
            
            OnModeChanged(value);
        }

        private void OnJumping()
        {
            _stamina.Remove(_jumpStaminaCost);
        }
        
        private void OnFallen(float height)
        {
            if (height < _deadlyHeight)
            {
                return;
            }
            
            ApplyDamage(gameObject, (height - _deadlyHeight) * _fallenDamageAmount);
        }

        protected override void OnDamageApplied(GameObject caller, float damage)
        {
            base.OnDamageApplied(caller, damage);

            var direction = (caller.transform.position - _transform.position).normalized;
            direction.y = 0f;

            if (Motor is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }
            
            ironPlayerMotor.AddForce(-direction * damage);
        }

        protected override void OnInteracting()
        {
            base.OnInteracting();
            
            if (Interaction.CurrentInteractive is not BaseTimeGrabInteractive timeGrabInteractive)
            {
                return;
            }

            _isTimingToInteract = true;

            _timeToInteract = 0f;
            _maxTimeToInteract = timeGrabInteractive.GetMaxTime();

            TimeGrabInteractive = timeGrabInteractive;
        }

        protected override void OnInteractionStopped()
        {
            base.OnInteractionStopped();
            
            if (_isTimingToInteract)
            {
                _timeToInteract = 0f;
                _isTimingToInteract = false;
            }

            TimeGrabInteractive = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (Motor is not IronPlayerMotor ironPlayerMotor)
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
                    TimeGrabInteractive.StopTimeInteraction();

                    TryStopInteraction();

                    _timeToInteract = _maxTimeToInteract;
                    _isTimingToInteract = false;
                }
                
                GrabbingTimeValueChanged?.Invoke(_timeToInteract);
            }

            if (Interaction.IsInteracting)
            {
                var currentInteractive = Interaction.CurrentInteractive;

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
            
            if (Motor is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }

            if (ironPlayerMotor.GetVelocity2D().magnitude > 0.02f)
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
                _stamina.Remove(_runningStaminaCost * deltaTime);
            }
            else
            {
                _stamina.Add(_staminaRest * deltaTime);
            }
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

        public void AddHealth(float amount)
        {
            WritableHealth.Add(amount);
        }

        public void AddStamina(float amount)
        {
            _stamina.Add(amount);
        }
        
        public bool GiveItem(string name, int count)
        {
            return _inventory.Place(name, count);
        }

        public bool RemoveItem(string name)
        {
            return _inventory.Remove(name);
        }

        public bool RemoveItem(string name, int count)
        {
            return _inventory.Remove(name, count);
        }

        public bool CheckExists(string name)
        {
            return _inventory.CheckExists(name);
        }

        public bool CheckExists(string name, int count)
        {
            return _inventory.CheckExists(name, count);
        }
        
        public bool Drop(string name, int count)
        {
            return _inventory.Drop(name, count);
        }

        public bool Interact(string name)
        {
            return _inventory.Interact(name);
        }
        
        public IReadableInventory Inventory => _inventory;

        public WeaponManager WeaponManager { get; private set; }
        public IReadableStamina ReadableStamina => _stamina;
        public BaseTimeGrabInteractive TimeGrabInteractive { get; private set; }
        public PlayerMode Mode { get; private set; }
    }
}