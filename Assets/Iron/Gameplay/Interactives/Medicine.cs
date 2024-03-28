using UnityEngine;

namespace Iron
{
    [CreateAssetMenu(fileName = "New Medicine", menuName = "Iron/Items/Medicine")]
    public class Medicine : Item
    {
        [SerializeField] private float _amount;
        
        protected override bool OnInteract(IronPlayer ironPlayer)
        {
            ironPlayer.AddHealth(_amount);
            
            return true;
        }
    }
}