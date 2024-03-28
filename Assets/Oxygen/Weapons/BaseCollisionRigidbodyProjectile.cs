using UnityEngine;

namespace Oxygen
{
    public class BaseCollisionRigidbodyProjectile : BaseRigidbodyProjectile
    {
        [SerializeField] private float _amount;
        [SerializeField] private bool _destroyOnCollision;

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (!collision.gameObject.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            {
                return;
            }
			
            damageReceiver.ApplyDamage(GetOwner(), _amount);

            if (_destroyOnCollision)
            {
                Destroy();
            }
        }
    }
}