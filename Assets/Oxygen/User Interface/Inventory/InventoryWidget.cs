using UnityEngine;

namespace Oxygen
{
    public class InventoryWidget : PageWidget
    {
        [SerializeField] private SlotWidget[] _drawerSlots;

        public void Construct(IItemCollector itemCollector)
        {
            var inventory = itemCollector.Inventory;
            
            inventory.Placed += OnInventoryPlaced;
            inventory.Removed += OnInventoryRemoved;
            
            inventory.RemovedAll += OnInventoryRemovedAll;
            
            OnConstruction(itemCollector);

            ItemCollector = itemCollector;
        }

        protected virtual void OnConstruction(IItemCollector itemCollector)
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

        private void OnInventoryRemovedAll(string name)
        {
            Remove(name);
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
                if (drawerSlot.IsLocked)
                {
                    continue;
                }
                
                if (!drawerSlot.IsAssigned)
                {
                    continue;
                }

                if (drawerSlot.Name != name)
                {
                    continue;
                }
                
                drawerSlot.Remove(count);

                return;
            }
        }

        private void Remove(string name)
        {
            foreach (var drawerSlot in _drawerSlots)
            {
                if (drawerSlot.IsLocked)
                {
                    continue;
                }

                if (!drawerSlot.IsAssigned)
                {
                    continue;
                }

                if (drawerSlot.Name != name)
                {
                    continue;
                }
                
                drawerSlot.Clear();
                
                return;
            }
        }

        private void Place(string name, int count)
        {
            SlotWidget firstFreeSlot = null;
            
            foreach (var drawerSlot in _drawerSlots)
            {
                if (drawerSlot.IsLocked)
                {
                    continue;
                }

                if (!drawerSlot.IsAssigned)
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

        protected virtual void OnClear()
        {
            
        }

        public void Clear()
        {
            ItemCollector.Inventory.Placed -= OnInventoryPlaced;
            
            ItemCollector.Inventory.Removed -= OnInventoryRemoved;
            ItemCollector.Inventory.RemovedAll -= OnInventoryRemovedAll;

            OnClear();

            ItemCollector = null;
        }
        
        protected IItemCollector ItemCollector { get; private set; }
    }
}