using Oxygen;
using UnityEngine;

namespace Iron
{
    public class Pickup : BaseInteractive
    {
        [SerializeField] private string _name;
        [SerializeField] private int _amount;
        
        protected override bool OnInteract(Player player)
        {
            if (player is not IronPlayer ironPlayer)
            {
                return false;
            }

            if (!ironPlayer.GiveItem(_name, _amount))
            {
                return false;
            }
            
            Destroy(gameObject);
                
            return true;

        }
    }
}