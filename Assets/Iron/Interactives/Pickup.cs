using Oxygen;
using UnityEngine;

namespace Iron
{
    public class Pickup : BaseInteractive
    {
        [SerializeField] private string _name;
        [SerializeField] private int _count;

        protected override bool OnInteract(Player player)
        {
            if (player is not IronPlayer ironPlayer)
            {
                return false;
            }

            if (!ironPlayer.GiveItem(_name, _count))
            {
                return false;
            }
            
            Destroy(gameObject);
                
            return true;
        }

        public void Stuck(int count)
        {
            _count = count;
        }
    }
}