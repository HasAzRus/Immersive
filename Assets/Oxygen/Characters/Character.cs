using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oxygen
{
	public class Character : Behaviour,
		IDamageReceiver,
		IKillable
	{
		public event Action<GameObject, float> DamageApplied;
		public event Action<GameObject> Died;

		public event Action<float> HealthChanged;
		
		[Header("Character")]
		
		[SerializeField] private float _maxHealth;
		[SerializeField] private bool _applyMaxHealth;
		
		[SerializeField] private bool _allowDamageReceive;

		private float _health;
		
		private bool _isDead;

		protected override void Start()
		{
			base.Start();

			if (_applyMaxHealth)
			{
				ApplyHealth(_maxHealth);
			}
		}

		private void SetHealth(float value)
		{
			_health = value;

			HealthChanged?.Invoke(value);
		}
		
		protected virtual void OnDamageApplied(GameObject caller,  float damage)
		{
			if (!_allowDamageReceive)
			{
				return;
			}
			
			var value = _health - damage;

			if (value > 0f)
			{
				SetHealth(value);
			}
			else
			{
				SetHealth(0);

				Kill(caller);
			}
		}

		protected virtual void OnDied(GameObject caller) 
		{ 

		}

		public void ApplyDamage(GameObject caller, float damage)
		{
			if (damage < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			
			OnDamageApplied(caller, damage);
			DamageApplied?.Invoke(caller, damage);
		}

		public void Kill(GameObject caller)
		{
			if(IsDead)
			{
				return;
			}

			_isDead = true;

			OnDied(caller);
			Died?.Invoke(caller);
		}

		public void ApplyHealth(float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			
			if (amount > _maxHealth)
			{
				SetHealth(_maxHealth);

				return;
			}
			
			SetHealth(amount);
		}

		public void AddHealth(float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			
			var value = _health + amount;
			
			ApplyHealth(value);
		}

		public float GetMaxHealth()
		{
			return _maxHealth;
		}

		public float GetHealth()
		{
			return _health;
		}

		public void SetAllowDamageReceive(bool value)
		{
			_allowDamageReceive = value;
		}

		public bool IsDead => _isDead;
	}
}