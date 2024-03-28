using UnityEngine;

namespace Oxygen
{
    public class BaseRigidbodyProjectile2D : BaseProjectile
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] protected Collider2D _collider;

        protected override void OnConstruct(GameObject ownerGameObject, float damage)
        {
            base.OnConstruct(ownerGameObject, damage);

            _rigidbody.isKinematic = false;

            if (ownerGameObject.TryGetComponent<Collider2D>(out var ownerCollider))
            {
                Physics2D.IgnoreCollision(_collider, ownerCollider, true);
            } 
        }

        protected override void OnLaunch(Vector3 direction)
        {
            _rigidbody.rotation = Vector2.Angle(Vector2.up, direction);
            _rigidbody.AddForce(direction, ForceMode2D.Impulse);
        }

        public Rigidbody2D GetRigidbody()
        {
            return _rigidbody;
        }
    }
}