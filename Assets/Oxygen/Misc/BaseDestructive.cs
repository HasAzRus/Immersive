using System;
using UnityEngine;

namespace Oxygen
{
	public abstract class BaseDestructive : BaseDamageable,
		IKillable
	{
		public event Action<GameObject> Died;

		[SerializeField] private Health _health;
		[SerializeField] private bool _destroyOnKilled;

		protected abstract void OnKilled(GameObject caller);

		protected override void OnDamageApplied(GameObject caller, float damage)
		{
			_health.Remove(damage);
		}

		protected override void Start()
		{
			base.Start();

			_health.Fill();
		}

		public bool Kill(GameObject caller)
		{
			OnKilled(caller);
			Died?.Invoke(caller);

			if(_destroyOnKilled)
			{
				Destroy(gameObject);
			}

			return true;
		}

		public IReadableHealth ReadableHealth => _health;
	}
}