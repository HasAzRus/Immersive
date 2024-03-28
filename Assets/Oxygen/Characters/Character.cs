using System;
using UnityEngine;

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
			Health = value;

			HealthChanged?.Invoke(value);
		}
		
		protected virtual void OnDamageApplied(GameObject caller,  float damage)
		{
			if (!_allowDamageReceive)
			{
				return;
			}
			
			var value = Health - damage;

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

			IsDead = true;

			OnDied(caller);
			Died?.Invoke(caller);
		}

		public void ApplyHealth(float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			SetHealth(Mathf.Clamp(amount, 1, _maxHealth));
		}

		public void AddHealth(float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			
			var value = Health + amount;
			
			ApplyHealth(value);
		}

		public void SetAllowDamageReceive(bool value)
		{
			_allowDamageReceive = value;
		}

		public bool IsDead { get; private set; }
		public float MaxHealth => _maxHealth;
		public float Health { get; private set; }
	}
}