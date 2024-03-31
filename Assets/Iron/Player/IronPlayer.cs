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
    [RequireComponent(typeof(IronPlayerFlashlight))]
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

        private WeaponManager _weaponManager;

        private IronPlayerFlashlight _flashlight;

        private Transform _transform;

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();

            _transform = transform;
            _bob = GetComponent<PlayerBob>();

            _inventory = GetComponent<IronPlayerInventory>();
            _inventory.Construct(this);

            _weaponManager = GetComponent<WeaponManager>();
            _weaponManager.Construct(this);

            _flashlight = GetComponent<IronPlayerFlashlight>();

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

        protected override void OnEnable()
        {
            base.OnEnable();
            
            SaveLoad.Saving += OnGameSaving;
            SaveLoad.Loading += OnGameLoading;
        }

        private void OnGameLoading(IReadableStorage storage)
        {
            var data = IronPlayerSaveLoad.Load(storage);
            
            WritableHealth.Apply(data.Health);
            _stamina.Apply(data.Stamina);
            _flashlight.AddEnergy(data.Energy);

            foreach (var item in data.Items)
            {
                _inventory.Place(item);
            }
        }

        private void OnGameSaving(IWritableStorage storage)
        {
            var data = new IronPlayerData()
            {
                Health = ReadableHealth.Value,
                Stamina = Stamina.Value,
                Energy = _flashlight.ReadableEnergy.Value,
                Items = _inventory.GetItems()
            };
            
            IronPlayerSaveLoad.Save(storage, data);
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
            
            Debug.Log($"Упал с высоты: {height}");
            
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
            _maxTimeToInteract = timeGrabInteractive.MaxTime;

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

        public bool HideWeapon()
        {
            return _weaponManager.DeselectCurrentWeapon();
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

        public int CheckExists(string name)
        {
            return _inventory.CheckExists(name);
        }

        public bool CheckExists(string name, int count)
        {
            return _inventory.CheckExists(name, count);
        }
        
        public bool Drop(string name)
        {
            return _inventory.Drop(name);
        }

        public bool Interact(string name)
        {
            return _inventory.Interact(name);
        }

        public void EnableFlashlight()
        {
            _flashlight.Enable();
        }

        public void DisableFlashlight()
        {
            _flashlight.Disable();
        }

        public void AddEnergy(float amount)
        {
            _flashlight.AddEnergy(amount);
        }

        public bool GiveWeapon(int index)
        {
            if (_weaponManager.CheckWeaponActive(index))
            {
                return false;
            }
            
            _weaponManager.SetWeaponActive(index, true);
            _weaponManager.ChangeWeapon(index);

            return true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            SaveLoad.Saving -= OnGameSaving;
            SaveLoad.Loading -= OnGameLoading;
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

        public IReadableInventory Inventory => _inventory;
        public IReadableWeaponManager WeaponManager => _weaponManager;
        public IReadableStamina Stamina => _stamina;
        public BaseTimeGrabInteractive TimeGrabInteractive { get; private set; }
        public PlayerMode Mode { get; private set; }
        public IReadableFlashlight Flashlight => _flashlight;
    }
}