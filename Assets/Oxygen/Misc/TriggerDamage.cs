using UnityEngine;

namespace Oxygen
{
    public class TriggerDamage : Behaviour
    {
        [SerializeField] private float _amount;
        [SerializeField] private bool _isDamageAlways;

        private bool _isTrigger;

        private IDamageReceiver _damageReceiver;

        protected override void Update()
        {
            base.Update();

            if (_isTrigger)
            {
                _damageReceiver.ApplyDamage(gameObject, _amount * Time.deltaTime);
            }
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if (!other.TryGetComponent(out IDamageReceiver damageReceiver))
            {
                return;
            }

            _damageReceiver = damageReceiver;

            if (_isDamageAlways)
            {
                _isTrigger = true;
                
                return;
            }
            
            _damageReceiver.ApplyDamage(gameObject, _amount);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            _isTrigger = false;
        }
    }
}