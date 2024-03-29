using UnityEngine;

namespace Iron
{
    [CreateAssetMenu(fileName = "New Drink", menuName = "Iron/Items/Drink")]
    public class Drink : ItemInteractive
    {
        [SerializeField] private float _amount;
        
        protected override bool OnInteract(IronPlayer ironPlayer)
        {
            ironPlayer.AddStamina(_amount);

            return true;
        }
    }
}