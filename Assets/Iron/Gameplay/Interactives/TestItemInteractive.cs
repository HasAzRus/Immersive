using UnityEngine;

namespace Iron
{
    [CreateAssetMenu(fileName = "New Test Item", menuName = "Iron/Items/Test Item")]
    public class TestItemInteractive : ItemInteractive
    {
        protected override bool OnInteract(IronPlayer ironPlayer)
        {
            Debug.Log("Тест предмет");
            
            return true;
        }
    }
}