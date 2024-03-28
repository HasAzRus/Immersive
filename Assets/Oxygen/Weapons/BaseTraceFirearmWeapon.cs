using System;
using UnityEngine;

namespace Oxygen
{
	public abstract class BaseTraceFirearmWeapon : BaseFirearmWeapon
	{
		public event Action<GameObject, float> Damaged;

		[Header("Tracer")]

		[SerializeField] private float _maxDistance;

		[SerializeField] private bool _allowMultiplier;
		[SerializeField] private bool _isInRow;

		[SerializeField] private float[] _offset;

		private int _index;

		private bool Trace(Vector3 position, Vector3 direction, float damageMultiplier)
		{
			if (!Physics.Raycast(position, direction, out RaycastHit hit,
				_maxDistance))
			{
				return false;
			}

			OnTraced(hit);

			if (!hit.collider.TryGetComponent<IDamageReceiver>(out var receiver))
			{
				return false;
			}

			var damage = GetDamage() * damageMultiplier;

			receiver.ApplyDamage(GetOwner(), damage);

			Damaged?.Invoke(hit.collider.gameObject, damage);
			OnDamaged(hit.collider.gameObject, damage);

			return true;
		}

		private bool TraceByOffset(int index, Vector3 offsetAxis, Vector3 position, Vector3 direction, float damageMultiplier)
		{
			return Trace(position + offsetAxis * _offset[index], direction, damageMultiplier);
		}

		protected virtual void OnDamaged(GameObject damageReceiver, float damage) 
		{

		}

		protected virtual void OnTraced(RaycastHit hit)
		{
			
		}

		protected bool Trace(float damageMultiplier)
		{
			var traceTransform = GetTrunkTransform();
			
			if (CheckAllowOwnerDirection())
			{
				traceTransform = GetOwnerTransform();
			}

			if(_allowMultiplier)
			{
				if(_isInRow)
				{
					if (!TraceByOffset(_index, traceTransform.right, traceTransform.position, traceTransform.forward,
						    damageMultiplier))
					{
						return false;
					}
					
					if (_index + 1 < _offset.Length)
					{
						_index += 1;
					}
					else
					{
						_index = 0;
					}

					return true;
				}
				else
				{
					bool traced = false;

					for(var i = 0; i < _offset.Length; i++)
					{
						traced |= TraceByOffset(i, traceTransform.right, traceTransform.position, traceTransform.forward, damageMultiplier);
					}

					return traced;
				}
			}
			else
			{
				return Trace(traceTransform.position, traceTransform.forward, damageMultiplier);
			}
		}

		protected override void OnShoot()
		{
			base.OnShoot();

			Trace(GetDamageMultiplier());
		}
	}
}