using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oxygen
{
	public abstract class BaseWeapon : Behaviour
	{
		public event Action<float> CooldownValueChanged;

		public event Action CooldownStarted;
		public event Action CooldownStopped;

		[Header("Base Weapon")]

		[SerializeField] private string _name;
		[SerializeField] private bool _isActive;

		[SerializeField] private float _damage;
		[SerializeField] private float _maxCooldownTime;

		[SerializeField] private float _fireSpeed;

		private float _cooldownTime;

		public void Construct(Character owner)
		{
			OnConstruct(owner);
			
			Owner = owner;
		}
		
		protected virtual void OnConstruct(Character owner)
		{
			
		}

		protected abstract bool OnFire(int mode);
		
		protected virtual void OnStopFire(int mode) 
		{
			
		}

		protected virtual void OnSelect()
		{

		}

		protected virtual void OnDeselect()
		{

		}

		protected override void Update()
		{
			base.Update();

			if (IsCooldown)
			{
				if (_cooldownTime < _maxCooldownTime)
				{
					_cooldownTime += Time.deltaTime * _fireSpeed;
				}
				else
				{
					_cooldownTime = 0f;
					IsCooldown = false;

					CooldownStopped?.Invoke();
				}

				CooldownValueChanged?.Invoke(_cooldownTime / _maxCooldownTime);
			}
		}

		protected void StartCooldown()
		{
			IsCooldown = true;

			CooldownStarted?.Invoke();
		}

		public bool Fire(int mode)
		{
			return OnFire(mode);
		}

		public void StopFire(int mode)
		{
			OnStopFire(mode);
		}

		public void Select()
		{
			IsSelected = true;

			OnSelect();
		}

		public void Deselect()
		{
			IsSelected = false;

			OnDeselect();
		}

		public void SetActive(bool value)
		{
			_isActive = value;
		}

		public void SetFireSpeed(float value)
		{
			_fireSpeed = value;
		}

		protected bool IsCooldown { get; private set; }

		public bool IsActive => _isActive;

		public bool IsSelected { get; private set; }

		public float Damage => _damage;

		public string Name => _name;

		public Character Owner { get; private set; }
	}
}