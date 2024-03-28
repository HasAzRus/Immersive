using UnityEngine;

namespace Oxygen
{
    public class BaseCollisionRigidbodyProjectile2D : BaseRigidbodyProjectile2D
    {
        [SerializeField] private float _amount;
        [SerializeField] private bool _destroyOnCollision;

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            base.OnCollisionEnter2D(other);

            if (!other.gameObject.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            {
                return;
            }
			
            damageReceiver.ApplyDamage(GetOwner(), _amount);

            if (_destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}