using System;
using UnityEngine;

namespace Oxygen
{
    [RequireComponent(typeof(LeverEventListener))]
    public class Lever : BaseInteractive
    {
        public event Action Activated;
        public event Action Deactivated;
        
        [SerializeField] private bool _initialActive;

        private bool _isActive;

        private void SetState(bool value)
        {
            _isActive = value;

            if (value)
            {
                Activated?.Invoke();
            }
            else
            {
                Deactivated?.Invoke();
            }
        }

        protected override void Start()
        {
            base.Start();
            
            SetState(_initialActive);
        }

        protected override bool OnInteract(Player player)
        {
            SetState(!_isActive);
            
            return true;
        }
    }
}