using Oxygen;
using UnityEngine;

namespace Iron
{
    public class WeaponPickup : BaseInteractive
    {
        [SerializeField] private Weapon _weapon;
        
        protected override bool OnInteract(Player player)
        {
            if (player is not IronPlayer ironPlayer)
            {
                return false;
            }

            if (!ironPlayer.GiveWeapon((int)_weapon))
            {
                return false;
            }
            
            Destroy(gameObject);

            return true;
        }
    }
}