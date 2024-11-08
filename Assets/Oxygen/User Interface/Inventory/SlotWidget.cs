using UnityEngine;

namespace Oxygen
{
    public class SlotWidget : Behaviour, IItem
    {
        public void Construct(InventoryWidget inventoryWidget)
        {
            OnConstruct(inventoryWidget);
            
            InventoryWidget = inventoryWidget;
        }

        protected virtual void OnConstruct(InventoryWidget inventoryWidget)
        {
            
        }

        protected virtual void OnAssigned(string name)
        {
            
        }

        protected virtual void OnClear()
        {
            
        }

        protected virtual void OnCountChanged(int value)
        {
            
        }

        protected virtual void OnLockedChanged(bool value)
        {
            
        }

        public void Assign(string name)
        {
            Name = name;
            IsAssigned = true;
            
            OnAssigned(name);
        }

        public void Clear()
        {
            Name = string.Empty;

            Count = 0;

            IsAssigned = false;

            OnClear();
        }

        public bool Add(int number)
        {
            if (!IsAssigned)
            {
                return false;
            }

            SetCount(Count + number);

            return true;
        }

        public bool Remove()
        {
            if (!IsAssigned)
            {
                return false;
            }

            SetCount(0);
            
            Clear();

            return true;
        }

        public bool Remove(int number)
        {
            if (!IsAssigned)
            {
                return false;
            }

            if (Count - number > 0)
            {
                SetCount(Count - number);
            }
            else
            {
                SetCount(0);
                
                Clear();
            }

            return true;
        }
        
        public void SetCount(int value)
        {
            Count = value;

            OnCountChanged(value);
        }
        
        public void SetLocked(bool value)
        {
            IsLocked = value;
            
            OnLockedChanged(value);
        }
        
        public string Name { get; private set; }
        public int Count { get; private set; }

        public bool IsAssigned { get; private set; }

        public bool IsLocked { get; private set; }

        protected InventoryWidget InventoryWidget { get; private set; }
    }
}