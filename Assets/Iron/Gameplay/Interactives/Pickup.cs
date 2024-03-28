using Oxygen;
using UnityEngine;

namespace Iron
{
    public class Pickup : BaseInteractive
    {
        [SerializeField] private string _name;
        [SerializeField] private int _amount;
        
        protected override bool OnInteract(Oxygen.Player player)
        {
            if (player is not IronPlayer ironPlayer)
            {
                return false;
            }

            if (ironPlayer.GetInventory().Place(_name, _amount))
            {
                Destroy(gameObject);
                
                return true;
            }

            return false;
        }
    }
}