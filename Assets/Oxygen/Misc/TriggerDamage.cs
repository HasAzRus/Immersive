using UnityEngine;

namespace Oxygen
{
    public class TriggerDamage : Behaviour
    {
        [SerializeField] private float _amount;
        [SerializeField] private bool _isDamageAlways;

        private bool _isTrigger;

        private IDamageable _damageable;

        protected override void Update()
        {
            base.Update();

            if (_isTrigger)
            {
                _damageable.ApplyDamage(gameObject, _amount * Time.deltaTime);
            }
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if (!other.TryGetComponent(out IDamageable damageReceiver))
            {
                return;
            }

            _damageable = damageReceiver;

            if (_isDamageAlways)
            {
                _isTrigger = true;
                
                return;
            }
            
            _damageable.ApplyDamage(gameObject, _amount);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            _isTrigger = false;
        }
    }
}