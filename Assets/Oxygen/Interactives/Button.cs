using UnityEngine;

namespace Oxygen
{
    [RequireComponent(typeof(InteractiveEventListener))]
    public class Button : BaseInteractive
    {
        protected override bool OnInteract(Player player)
        {
            return true;
        }
    }
}