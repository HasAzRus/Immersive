using UnityEngine;

namespace Oxygen
{
	public class Explosion : Behaviour
	{
		[SerializeField] private float _maxTime;

		[SerializeField] private SphereCollider _collider;

		private float _time;

		private float _damage;

		public void Constuct(float radius, float damage)
		{
			_damage = damage;

			_collider.radius = radius;
		}

		protected override void Update()
		{
			base.Update();

			if(_time < _maxTime)
			{
				_time += Time.deltaTime;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		protected override void OnTriggerEnter(Collider other)
		{
			base.OnTriggerEnter(other);

			var cachedTransform = transform;

			var distance = Vector3.Distance(cachedTransform.position, other.transform.position);
			var amount = 1f - (distance / _collider.radius);

			if (other.TryGetComponent<IDamageReceiver>(out var damageReceiver))
			{
				damageReceiver.ApplyDamage(gameObject, _damage * amount);
			}
		}
	}
}