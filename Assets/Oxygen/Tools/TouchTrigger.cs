using System;
using UnityEngine;

namespace Oxygen
{
    public enum TouchTriggerMode
    {
        Enter,
        Exit,
        Both
    }
    
    [RequireComponent(typeof(TouchTriggerEventListener))]
    public sealed class TouchTrigger : BaseTouchTrigger
    {
        public event Action<Player> Triggered;
        public event Action<Player> Untriggered;
        
        [SerializeField] private TouchTriggerMode _mode;
        [SerializeField] private bool _onlyOnce;


        protected override void OnTriggered(Player player)
        {
            var checkOnlyOnce = true;

            if (_mode != TouchTriggerMode.Enter)
            {
                if (_mode != TouchTriggerMode.Both)
                {
                    return;
                }

                checkOnlyOnce = false;
            }
            
            Triggered?.Invoke(player);
            
            if (!checkOnlyOnce)
            {
                return;
            }
            
            if (_onlyOnce)
            {
                Destroy(gameObject);
            }
        }

        protected override void OnUntriggered(Player player)
        {
            base.OnUntriggered(player);
            
            if (_mode != TouchTriggerMode.Exit)
            {
                if (_mode != TouchTriggerMode.Both)
                {
                    return;
                }
            }
            
            Untriggered?.Invoke(player);
            
            if (_onlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}