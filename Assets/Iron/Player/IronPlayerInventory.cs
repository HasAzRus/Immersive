using Oxygen;
using UnityEngine;

namespace Iron
{
    public class IronPlayerInventory : PlayerInventory
    {
        [SerializeField] private float _dropForce;
        [SerializeField] private Transform _dropTransform;

        [SerializeField] private ItemDatabase _itemDatabase;

        private IronPlayer _ironPlayer;

        public void Construct(IronPlayer ironPlayer)
        {
            _ironPlayer = ironPlayer;
        }

        public bool Drop(string name)
        {
            if (!_itemDatabase.TryGetValue(name, out var itemData))
            {
                return false;
            }

            var count = itemData.Count;

            if (!Remove(name, count))
            {
                var remainCount = CheckExists(name);

                if (!Remove(name))
                {
                    return false;
                }

                count = remainCount;
            }

            var dropPickup = Instantiate(itemData.Prefab, _dropTransform.position, _dropTransform.rotation);
            dropPickup.Stuck(count);
            
            var dropRigidbody = dropPickup.GetComponent<Rigidbody>();

            dropRigidbody.AddForce(_dropTransform.forward * _dropForce, ForceMode.Impulse);

            return true;
        }

        public bool Interact(string name)
        {
            if (!_itemDatabase.TryGetValue(name, out var itemData))
            {
                return false;
            }

            if (itemData.Interactive == null)
            {
                return false;
            }

            return itemData.Interactive.Interact(_ironPlayer);
        }
    }
}