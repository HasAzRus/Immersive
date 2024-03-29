using System;
using UnityEngine;

namespace Oxygen
{
	public abstract class Character : Behaviour,
		IDamageable,
		IKillable
	{
		public event Action<GameObject, float> DamageApplied;
		public event Action<GameObject> Died;

		[Header("Character")] 
		
		[SerializeField] private Health _health;
		[SerializeField] private bool _applyMaxHealth;
		
		[SerializeField] private bool _allowDamageReceive;

		protected override void Start()
		{
			base.Start();

			if (_applyMaxHealth)
			{
				_health.Fill();
			}
		}
		
		protected virtual void OnDamageApplied(GameObject caller,  float damage)
		{

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
			
			if (!_allowDamageReceive)
			{
				return;
			}
			
			_health.Remove(damage);

			if (_health.IsEmpty)
			{
				Kill(caller);
			}

			OnDamageApplied(caller, damage);
			DamageApplied?.Invoke(caller, damage);
		}

		public bool Kill(GameObject caller)
		{
			if(IsDead)
			{
				return false;
			}

			IsDead = true;

			OnDied(caller);
			Died?.Invoke(caller);

			return true;
		}

		protected IWritableHealth WritableHealth => _health;
		public bool IsDead { get; private set; }
		public IReadableHealth ReadableHealth => _health;
	}
}