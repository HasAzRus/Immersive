using System;
using UnityEngine;

namespace Oxygen
{
	public enum FireMode
	{
		Single,
		Automatic
	}

	[Serializable]
	public class MuzzleFlash
	{
		[SerializeField] private float _maxTime;

		[SerializeField] private Light _light;

		private bool _isFlashing;
		private float _time;

		public void Update()
		{
			if (_isFlashing)
			{
				if (_time < _maxTime)
				{
					_time += Time.deltaTime;
				}
				else
				{
					Clear();
				}

				_light.intensity = 1.0f - _time / _maxTime;
			}
		}

		public void Flash()
		{
			_isFlashing = true;

			_light.enabled = true;
		}

		public void Clear()
		{
			_isFlashing = false;
			_time = 0f;

			_light.enabled = false;
		}
	}

	[Serializable]
	public class FireImpact
	{
		[SerializeField] private Range _horizontal;
		[SerializeField] private Range _vertical;

		public Range GetHorizontal()
		{
			return _horizontal;
		}

		public Range GetVertical()
		{
			return _vertical;
		}
	}

	public abstract class BaseFirearmWeapon : BaseWeapon
	{
		public event Action<FireImpact> Impacting;
		public event Action<int> AmmoChanged;

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

		private int _ammo;

		private bool _isFiring;

		private void SetAmmo(int value)
		{
			_ammo = value;

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

		protected virtual void OnImpacting(FireImpact impact)
		{
			
		}

		protected virtual void OnShoot()
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

			_muzzleFlash.Update();

			if (!CheckActive())
			{
				return;
			}

			if (!CheckSelected())
			{
				return;
			}

			if (CheckCooldown())
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

		protected override void OnClear()
		{
			base.OnClear();

			SetAmmo(0);
		}

		protected override bool OnFire(int mode)
		{
			switch (mode)
			{
				case 0:
				{
					if (_ammo == 0)
					{
						if (!_isInfinity)
						{
							return false;
						}
					}

					if (CheckCooldown())
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
					break;
			}
		}

		protected void ConsumeAmmo(int amount)
		{
			if (_ammo - amount > 0)
			{
				SetAmmo(_ammo - amount);
			}
			else
			{
				SetAmmo(0);

				OnConsumed();
			}
		}

		protected bool CheckAllowOwnerDirection()
		{
			return _allowOwnerDirection;
		}

		protected Transform GetTrunkTransform()
		{
			return _trunkTransform;
		}

		public int Reload(int ammo)
		{
			if (_ammo == _maxAmmo)
			{
				return 0;
			}

			var amount = _maxAmmo - _ammo;

			if (amount < ammo)
			{
				SetAmmo(_ammo + amount);

				return amount;
			}
			else
			{
				SetAmmo(_ammo + ammo);

				return ammo;
			}
		}

		public void SetMaxAmmo(int value)
		{
			_maxAmmo = value;
		}

		public int GetAmmo()
		{
			return _ammo;
		}

		public int GetMaxAmmo()
		{
			return _maxAmmo;
		}

		public float GetDamageMultiplier()
		{
			return _damageMultiplier;
		}
	}
}