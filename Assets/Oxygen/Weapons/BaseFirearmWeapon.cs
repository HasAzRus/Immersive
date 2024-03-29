using System;
using UnityEngine;

namespace Oxygen
{
	public enum FireMode
	{
		Single,
		Automatic
	}
	
	public abstract class BaseFirearmWeapon : BaseWeapon
	{
		public event Action<FireImpact> Impacting;
		public event Action<int> AmmoChanged;

		public event Action Aiming;
		public event Action AimStopped;
		
		[Header("Firearm")] 
		
		[SerializeField] private bool _isInfinity;
		
		[SerializeField] private int _maxAmmo;
		[SerializeField] private int _damageMultiplier;

		[SerializeField] private FireMode _mode;

		[SerializeField] private bool _allowMuzzleFlash;
		[SerializeField] private MuzzleFlash _muzzleFlash;
		
		[SerializeField] private FireImpact _impact;

		[SerializeField] private bool _allowOwnerDirection;
		[SerializeField] private Transform _trunkTransform;

		private bool _isAiming;

		private bool _isFiring;

		private void SetAmmo(int value)
		{
			Ammo = value;

			AmmoChanged?.Invoke(value);
		}

		private void Shoot()
		{
			OnImpacting(_impact);
			Impacting?.Invoke(_impact);

			if (_allowMuzzleFlash)
			{
				_muzzleFlash.Flash();
			}

			OnShoot();

			StartCooldown();
		}

		private void StartAiming()
		{
			_isAiming = true;
			
			OnAiming();
			Aiming?.Invoke();
		}

		private void StopAiming()
		{
			_isAiming = false;
			
			OnAimStopped();
			AimStopped?.Invoke();
		}

		protected virtual void OnImpacting(FireImpact impact)
		{
			
		}

		protected virtual void OnShoot()
		{

		}

		protected virtual void OnAiming()
		{
			
		}

		protected virtual void OnAimStopped()
		{
			
		}

		protected virtual void OnConsumed()
		{
			StopFire(0);
		}

		protected override void Start()
		{
			base.Start();

			if (_allowMuzzleFlash)
			{
				_muzzleFlash.Clear();
			}
		}

		protected override void Update()
		{
			base.Update();

			if (_allowMuzzleFlash)
			{
				_muzzleFlash.Update();
			}

			if (!IsActive)
			{
				return;
			}

			if (!IsSelected)
			{
				return;
			}

			if (IsCooldown)
			{
				return;
			}

			if (_mode != FireMode.Automatic)
			{
				return;
			}

			if (!_isFiring)
			{
				return;
			}
			
			if (!_isInfinity)
			{
				ConsumeAmmo(_damageMultiplier);
			}

			Shoot();
		}

		protected override bool OnFire(int mode)
		{
			switch (mode)
			{
				case 0:
				{
					if (Ammo == 0)
					{
						if (!_isInfinity)
						{
							return false;
						}
					}

					if (IsCooldown)
					{
						return false;
					}

					switch (_mode)
					{
						case FireMode.Single:
						{
							if (!_isInfinity)
							{
								ConsumeAmmo(_damageMultiplier);
							}

							Shoot();
							break;
						}
						case FireMode.Automatic:
							_isFiring = true;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					break;
				}
				case 1:
					if (!_isFiring)
					{
						StartAiming();
					}
					
					break;
			}

			return true;
		}

		protected override void OnStopFire(int mode)
		{
			base.OnStopFire(mode);

			switch (mode)
			{
				case 0:
				{
					if (_mode == FireMode.Automatic)
					{
						_isFiring = false;
					}

					break;
				}
				case 1:
					if (_isAiming)
					{
						StopAiming();
					}
					
					break;
			}
		}

		protected void ConsumeAmmo(int amount)
		{
			if (Ammo - amount > 0)
			{
				SetAmmo(Ammo - amount);
			}
			else
			{
				SetAmmo(0);

				OnConsumed();
			}
		}

		public int Reload(int ammo)
		{
			if (Ammo == _maxAmmo)
			{
				return 0;
			}

			var amount = _maxAmmo - Ammo;

			if (amount < ammo)
			{
				SetAmmo(Ammo + amount);

				return amount;
			}

			SetAmmo(Ammo + ammo);

			return ammo;
		}

		public void SetMaxAmmo(int value)
		{
			_maxAmmo = value;
		}

		public bool AllowOwnerDirection => _allowOwnerDirection;

		public Transform Trunk => _trunkTransform;

		public int Ammo { get; private set; }
		public int MaxAmmo => _maxAmmo;
		
		public float DamageMultiplier => _damageMultiplier;
	}
}