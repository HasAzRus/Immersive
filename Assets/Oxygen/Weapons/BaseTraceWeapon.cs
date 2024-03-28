using UnityEngine;

namespace Oxygen
{
	public abstract class BaseTraceWeapon : BaseWeapon
	{
		[Header("Tracer")]

		[SerializeField] private float _maxDistance;

		protected virtual void OnTraced(RaycastHit hit)
		{

		}

		protected bool Trace(float damageMultiplier)
		{
			var traceTransform = GetOwnerTransform();
			
			if (!Physics.Raycast(traceTransform.position, traceTransform.forward, out RaycastHit hit,
				    _maxDistance))
			{
				return false;
			}

			OnTraced(hit);

			if (!hit.collider.TryGetComponent<IDamageReceiver>(out var receiver))
			{
				return false;
			}

			receiver.ApplyDamage(traceTransform.gameObject, GetDamage() * damageMultiplier);

			return true;
		}
	}
}