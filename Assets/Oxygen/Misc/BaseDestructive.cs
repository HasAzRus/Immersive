using System;
using UnityEngine;

namespace Oxygen
{
	public abstract class BaseDestructive : BaseDamageReceiver,
		IKillable
	{
		public event Action<GameObject> Killed;
		public event Action<float> HealthChanged;

		[SerializeField] private float _maxHealth;
		[SerializeField] private bool _destroyOnKilled;

		private float _health;

		private void SetHealth(float value)
		{
			_health = value;

			OnHealthChanged(_health);
			HealthChanged?.Invoke(_health);
		}

		protected abstract void OnKilled(GameObject caller);

		protected virtual void OnHealthChanged(float value)
		{

		}
		
		protected override void OnDamageApplied(GameObject caller, float damage)
		{
			if(_health - damage > 0)
			{
				SetHealth(_health - damage);
			}
			else
			{
				SetHealth(0);

				Kill(caller);
			}
		}

		protected override void Start()
		{
			base.Start();

			SetHealth(_maxHealth);
		}

		public void Kill(GameObject caller)
		{
			OnKilled(caller);
			Killed?.Invoke(caller);

			if(_destroyOnKilled)
			{
				Destroy(gameObject);
			}
		}
	}
}