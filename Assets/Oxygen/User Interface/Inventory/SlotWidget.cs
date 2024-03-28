using UnityEngine;

namespace Oxygen
{
    public class SlotWidget : Behaviour, IItem
    {
        public string Name { get; private set; }
        public int Count { get; private set; }

        private int _previousCount;

        private bool _isAssigned;

        private bool _isLocked;

        private InventoryWidget _inventoryWidget;
        
        public void Construct(InventoryWidget inventoryWidget)
        {
            OnConstruct(inventoryWidget);
            
            _inventoryWidget = inventoryWidget;
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
            _isAssigned = true;
            
            OnAssigned(name);
        }

        public void Clear()
        {
            Name = string.Empty;

            Count = 0;
            _previousCount = 0;
            
            _isAssigned = false;

            OnClear();
        }

        public bool Add(int number)
        {
            if (!_isAssigned)
            {
                return false;
            }

            SetCount(Count + number);

            return true;
        }

        public bool Remove()
        {
            if (!_isAssigned)
            {
                return false;
            }

            SetCount(0);
            
            Clear();

            return true;
        }

        public bool Remove(int number)
        {
            if (!_isAssigned)
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
            _previousCount = Count;
            Count = value;

            OnCountChanged(value);
        }

        public int GetPreviousCount()
        {
            return _previousCount;
        }

        public bool CheckAssigned()
        {
            return _isAssigned;
        }

        public void SetLocked(bool value)
        {
            _isLocked = value;
            
            OnLockedChanged(value);
        }

        public bool CheckLocked()
        {
            return _isLocked;
        }

        public InventoryWidget GetInventory()
        {
            return _inventoryWidget;
        }
    }
}