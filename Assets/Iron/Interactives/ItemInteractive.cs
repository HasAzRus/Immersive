using Oxygen;
using UnityEngine;

namespace Iron
{
    public abstract class ItemInteractive : ScriptableObject
    {
        [SerializeField] private string _name;

        protected abstract bool OnInteract(IronPlayer ironPlayer);
        
        public bool Interact(Player player)
        {
            if (player is not IronPlayer ironPlayer)
            {
                return false;
            }
            
            if (OnInteract(ironPlayer))
            {
                ironPlayer.RemoveItem(_name, 1);
            }

            return false;
        }
    }
}