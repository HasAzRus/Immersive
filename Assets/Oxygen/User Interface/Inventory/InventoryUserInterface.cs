using UnityEngine;

namespace Oxygen
{
    public class InventoryUserInterface : UserInterfacePage
    {
        [SerializeField] private SlotDrawer[] _drawerSlots;

        private PlayerInventory _inventory;
        
        public void Construct(PlayerInventory inventory)
        {
            inventory.Placed += OnInventoryPlaced;
            inventory.Removed += OnInventoryRemoved;
            
            inventory.RemovedAll += OnInventoryRemovedAll;
            
            OnConstruct(inventory);
            
            _inventory = inventory;
        }

        private void OnInventoryRemovedAll(string name)
        {
            
        }

        private void OnInventoryRemoved(string name, int count)
        {
            Remove(name, count);
        }

        private void OnInventoryPlaced(string name, int count)
        {
            Place(name, count);
        }
        
        private void Remove(string name, int count)
        {
            foreach (var drawerSlot in _drawerSlots)
            {
                if (drawerSlot.CheckLocked())
                {
                    continue;
                }
                
                if (!drawerSlot.CheckAssigned())
                {
                    continue;
                }

                if (drawerSlot.Name != name)
                {
                    continue;
                }

                if (count > 0)
                {
                    drawerSlot.Remove(count);
                }
                else
                {
                    drawerSlot.Clear();
                }

                return;
            }
        }

        private void Place(string name, int count)
        {
            SlotDrawer firstFreeSlot = null;
            
            foreach (var drawerSlot in _drawerSlots)
            {
                if (drawerSlot.CheckLocked())
                {
                    continue;
                }

                if (!drawerSlot.CheckAssigned())
                {
                    if (firstFreeSlot == null)
                    {
                        firstFreeSlot = drawerSlot;
                    }
                    
                    continue;
                }

                if (drawerSlot.Name != name)
                {
                    continue;
                }

                drawerSlot.Add(count);

                return;
            }

            if (firstFreeSlot == null)
            {
                return;
            }
            
            firstFreeSlot.Assign(name);
            firstFreeSlot.Add(count);
        }

        protected virtual void OnConstruct(PlayerInventory inventory)
        {
            
        }

        protected virtual void OnClear()
        {
            
        }

        protected override void Start()
        {
            base.Start();

            foreach (var slotDrawer in _drawerSlots)
            {
                slotDrawer.Construct(this);
            }
        }

        protected PlayerInventory GetInventory()
        {
            return _inventory;
        }

        public void Clear()
        {
            _inventory.Placed -= OnInventoryPlaced;
            
            _inventory.Removed -= OnInventoryRemoved;
            _inventory.RemovedAll -= OnInventoryRemovedAll;

            OnClear();

            _inventory = null;
        }
    }
}