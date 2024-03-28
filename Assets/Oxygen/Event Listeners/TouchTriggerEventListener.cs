using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
    public class TouchTriggerEventListener : Behaviour
    {
        [SerializeField] private TouchTrigger _target;

        [SerializeField] private UnityEvent<Player> _onTriggeredEvent;
        [SerializeField] private UnityEvent<Player> _onUntriggeredEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _target.Triggered += OnTriggered;
            _target.Untriggered += OnUntriggered;
        }

        private void OnTriggered(Player player)
        {
            _onTriggeredEvent.Invoke(player);
        }
        
        private void OnUntriggered(Player player)
        {
            _onUntriggeredEvent.Invoke(player);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _target.Triggered -= OnTriggered;
            _target.Untriggered -= OnUntriggered;
        }
    }
}