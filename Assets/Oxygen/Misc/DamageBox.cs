using UnityEngine;

namespace Oxygen
{
	[RequireComponent(typeof(Rigidbody))]
	public class DamageBox : Behaviour, IDamageReceiver
	{
		private Transform _transform;

		private Rigidbody _rigidbody;

		protected override void Start()
		{
			base.Start();

			_transform = transform;

			_rigidbody = GetComponent<Rigidbody>();
		}
		public void ApplyDamage(GameObject caller, float damage)
		{
			var directionFromCaller = (_transform.position - caller.transform.position).normalized;

			_rigidbody.AddForce(directionFromCaller * damage, ForceMode.Impulse);
		}
	}
}