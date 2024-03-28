using UnityEngine;

namespace Oxygen
{
	[RequireComponent(typeof(Rigidbody))]
	public class BreakBoxRigidbody : BreakBox
	{
		private Rigidbody _rigidbody;

		protected override void OnDamageApplied(GameObject caller, float damage)
		{
			base.OnDamageApplied(caller, damage);

			var directionFromCaller = (GetTransform().position - caller.transform.position).normalized;

			_rigidbody.AddForce(directionFromCaller * damage, ForceMode.Impulse);
		}

		protected override void Start()
		{
			base.Start();

			_rigidbody = GetComponent<Rigidbody>();
		}
	}
}