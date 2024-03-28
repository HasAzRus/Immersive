using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
    public class InteractiveEventListener : Behaviour
    {
        [SerializeField] private BaseInteractive _target;

        [SerializeField] private UnityEvent<Player> _onInteractEvent;
        [SerializeField] private UnityEvent<Player> _onInteractFailedEvent;
        
        private void OnFailed(Player player)
        {
            _onInteractFailedEvent.Invoke(player);
        }

        private void OnInteracting(Player player)
        {
            _onInteractEvent.Invoke(player);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _target.Interacting += OnInteracting;
            _target.Failed += OnFailed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _target.Interacting -= OnInteracting; 
            _target.Failed -= OnFailed;
        }
    }
}