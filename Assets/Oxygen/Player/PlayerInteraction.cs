using UnityEngine;

namespace Oxygen
{
    public abstract class PlayerInteraction : Behaviour
    {
        public bool IsEnabled;

        protected abstract bool TryGetInteractive(out IInteractive interactive);
        
        public bool TryInteract(Player player)
        {
            if (!IsEnabled)
            {
                return false;
            }

            if (!TryGetInteractive(out var interactive))
            {
                return false;
            }

            if (!interactive.IsEnabled)
            {
                return false;
            }

            if (!interactive.Interact(player))
            {
                return false;
            }
            
            IsInteracting = true;
            CurrentInteractive = interactive;

            return true;
        }

        public bool TryStopInteraction(Player player)
        {
            if (!IsInteracting)
            {
                return false;
            }

            IsInteracting = false;
            
            CurrentInteractive.StopInteraction(player);
            CurrentInteractive = null;

            return true;
        }
        
        public IInteractive CurrentInteractive { get; private set; }
        public bool IsInteracting { get; private set; }
    }
}