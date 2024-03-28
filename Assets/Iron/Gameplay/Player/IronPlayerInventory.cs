using Oxygen;
using UnityEngine;

namespace Iron
{
    public class IronPlayerInventory : PlayerInventory
    {
        [SerializeField] private float _dropForce;
        [SerializeField] private Transform _dropTransform;
        
        [SerializeField] private ItemDropDatabase _itemDropDatabase;
        [SerializeField] private ItemInteractionDatabase _itemInteractionDatabase;

        private IronPlayer _ironPlayer;

        public void Construct(IronPlayer ironPlayer)
        {
            _ironPlayer = ironPlayer;
        }

        public bool Drop(string name, int count)
        {
            if (!_itemDropDatabase.TryGetValue(name, out var original))
            {
                return false;
            }

            if (!Remove(name, count))
            {
                return false;
            }

            var dropGameObject = Instantiate(original, _dropTransform.position, _dropTransform.rotation);
            var dropRigidbody = dropGameObject.GetComponent<Rigidbody>();

            dropRigidbody.AddForce(_dropTransform.forward * _dropForce, ForceMode.Impulse);

            return true;
        }

        public bool Interact(string name)
        {
            if (!_itemInteractionDatabase.TryGetValue(name, out var itemInteractive))
            {
                return false;
            }

            return itemInteractive.Interact(_ironPlayer);
        }
    }
}