﻿using System;
using UnityEngine;

namespace Oxygen
{
	public abstract class BaseDamageable : Behaviour,
		IDamageable
	{
		public event Action<GameObject, float> DamageApplied;

		protected abstract void OnDamageApplied(GameObject caller, float damage);

		public void ApplyDamage(GameObject caller, float damage)
		{
			OnDamageApplied(caller, damage);
			DamageApplied?.Invoke(caller, damage);
		}
	}
}