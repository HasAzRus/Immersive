using Oxygen;
using UnityEngine;
using Behaviour = Oxygen.Behaviour;

namespace Iron
{
    public class ItemReceiver : BaseInteractive
    {
        [SerializeField] private SerializeItem[] _items;
        
        protected override bool OnInteract(Player player)
        {
            if (player is not IronPlayer ironPlayer)
            {
                return false;
            }
            
            foreach (var item in _items)
            {
                
            }

            return true;
        }
    }
}