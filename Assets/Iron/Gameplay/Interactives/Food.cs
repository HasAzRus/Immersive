using UnityEngine;

namespace Iron
{
    [CreateAssetMenu(fileName = "New Food", menuName = "Iron/Items/Food")]
    public class Food : Item
    {
        [SerializeField] private float _health;
        [SerializeField] private float _stamina;
        
        protected override bool OnInteract(IronPlayer ironPlayer)
        {
            ironPlayer.AddHealth(_health);
            ironPlayer.AddStamina(_stamina);
            
            return true;
        }
    }
}