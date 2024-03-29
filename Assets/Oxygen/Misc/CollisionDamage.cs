using UnityEngine;

namespace Oxygen
{
    public class CollisionDamage : Behaviour
    {
        [SerializeField] private float _amount;
        [SerializeField] private bool _isDamageAlways;

        private IDamageable _damageable;
        
        private bool _isCollision;

        protected override void Update()
        {
            base.Update();

            if (_isCollision)
            {
                _damageable.ApplyDamage(gameObject, _amount * Time.deltaTime);
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (!collision.gameObject.TryGetComponent(out IDamageable damageReceiver))
            {
                return;
            }

            _damageable = damageReceiver;
            
            if (_isDamageAlways)
            {
                _isCollision = true;
                
                return;
            }
            
            damageReceiver.ApplyDamage(gameObject, _amount);
        }

        protected override void OnCollisionExit(Collision collision)
        {
            base.OnCollisionExit(collision);

            _isCollision = false;
        }
    }
}