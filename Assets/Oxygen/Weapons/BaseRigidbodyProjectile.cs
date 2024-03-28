using UnityEngine;

namespace Oxygen
{
	public abstract class BaseRigidbodyProjectile : BaseProjectile
	{
		[SerializeField] private Rigidbody _rigidbody;

		[SerializeField] protected Collider _collider;

		protected override void OnConstruct(GameObject ownerGameObject, float damage)
		{
			base.OnConstruct(ownerGameObject, damage);

			_rigidbody.isKinematic = false;

			if (ownerGameObject.TryGetComponent<Collider>(out var ownerCollider))
			{
				Physics.IgnoreCollision(_collider, ownerCollider, true);
			} 
		}

		protected override void OnLaunch(Vector3 direction)
		{
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			
			_rigidbody.rotation = Quaternion.LookRotation(direction);
			
			_rigidbody.AddForce(direction, ForceMode.Impulse);
		}

		public Rigidbody GetRigidbody()
		{
			return _rigidbody;
		}
	}
}