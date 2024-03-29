using UnityEngine;

namespace Iron
{
    [CreateAssetMenu(fileName = "New Battery", menuName = "Iron/Items/Battery")]
    public class Battery : ItemInteractive
    {
        [SerializeField] private float _amount;
        
        protected override bool OnInteract(IronPlayer ironPlayer)
        {
            ironPlayer.AddEnergy(_amount);

            return true;
        }
    }
}