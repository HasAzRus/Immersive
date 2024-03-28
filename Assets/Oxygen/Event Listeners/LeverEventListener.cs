using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
    public class LeverEventListener : Behaviour
    {
        [SerializeField] private Lever _target;

        [SerializeField] private UnityEvent _onActivatedEvent;
        [SerializeField] private UnityEvent _onDeactivatedEvent;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _target.Activated += OnActivated;
            _target.Deactivated += OnDeactivated;
        }

        private void OnDeactivated()
        {
            _onDeactivatedEvent.Invoke();
        }

        private void OnActivated()
        {
            _onActivatedEvent.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _target.Activated -= OnActivated;
            _target.Deactivated -= OnDeactivated;
        }
    }
}