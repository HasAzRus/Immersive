using System;
using UnityEngine;

namespace Oxygen
{
    public interface IItem
    {
        string Name { get; }
        int Count { get; }
    }

    public interface ISlot : IItem
    {
        int GetPreviousCount();
    }
    
    [Serializable]
    public class Item : IItem
    {
        [SerializeField] private string _name;
        [SerializeField] private int _count;

        public string Name => _name;
        public int Count => _count;

        public override string ToString()
        {
            return $"Предмет: {_name} в кол-ве - {_count}";
        }
    }
    
    [Serializable]
    public class Slot : ISlot
    {
        public string Name { get; private set; }
        public int Count { get; private set; }
        
        private int _previousCount;

        private bool _isAssigned;
        private bool _isLocked;

        public void Assign(string name)
        {
            Name = name;
            _isAssigned = true;
        }

        public void Clear()
        {
            Name = string.Empty;
            _isAssigned = false;
            
            SetCount(0);
            _previousCount = 0;
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
        }

        public int GetPreviousCount()
        {
            return _previousCount;
        }

        public bool CheckAssigned()
        {
            return _isAssigned;
        }

        public bool CheckLocked()
        {
            return _isLocked;
        }

        public void SetLocked(bool value)
        {
            _isLocked = value;
        }
    }
    
    public class PlayerInventory : Behaviour
    {
        public event Action<string, int> Placed;
        public event Action<string, int> Removed;
        
        public event Action<string> RemovedAll;
        
        [SerializeField] private int _capacity;

        private Slot[] _slots;

        protected override void Awake()
        {
            base.Awake();
            
            _slots = new Slot[_capacity];

            for (var i = 0; i < _capacity; i++)
            {
                var slot = new Slot();
                //slot.SetLocked(false);
                
                _slots[i] = slot;
            }
        }

        public bool Place(string name, int count)
        {
            foreach (var slot in _slots)
            {
                if (slot.CheckLocked())
                {
                    continue;
                }
                
                if (slot.CheckAssigned())
                {
                    if (slot.Name != name)
                    {
                        continue;
                    }
                }
                else
                {
                    slot.Assign(name);
                }

                slot.Add(count);
                
                Debug.Log($"Был добавлен предмет {name} в кол-ве {count}");
                
                Placed?.Invoke(name, count);

                return true;
            }

            return false;
        }

        public bool Place(IItem item)
        {
            return Place(item.Name, item.Count);
        }

        public bool Remove(string name)
        {
            foreach (var slot in _slots)
            {
                if (slot.CheckLocked())
                {
                    continue;
                }
                
                if (!slot.CheckAssigned())
                {
                    continue;
                }

                if (slot.Name == name)
                {
                    continue;
                }
                
                RemovedAll?.Invoke(name);

                slot.Remove();
                
                Debug.Log($"Предмет был удален: {name}");

                return true;
            }

            return false;
        }

        public bool Remove(string name, int count)
        {
            foreach (var slot in _slots)
            {
                if (slot.CheckLocked())
                {
                    continue;
                }
                
                if (!slot.CheckAssigned())
                {
                    continue;
                }

                if (slot.Name != name)
                {
                    continue;
                }
                
                Removed?.Invoke(name, count);
                slot.Remove(count);
                
                Debug.Log($"Предмет был удален в кол-ве: {name} - {count}");

                return true;
            }

            return false;
        }

        public bool Remove(IItem item)
        {
            return Remove(item.Name, item.Count);
        }

        public bool CheckIsHave(string name)
        {
            foreach (var slot in _slots)
            {
                if (slot.CheckLocked())
                {
                    continue;
                }
                
                if (!slot.CheckAssigned())
                {
                    continue;
                }

                if (slot.Name != name)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public bool CheckIsHave(string name, int count)
        {
            foreach (var slot in _slots)
            {
                if (slot.CheckLocked())
                {
                    continue;
                }
                
                if (!slot.CheckAssigned())
                {
                    continue;
                }

                if (slot.Name != name)
                {
                    continue;
                }

                if (slot.Count < count)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public bool CheckIsHave(IItem item)
        {
            return CheckIsHave(item.Name, item.Count);
        }

        public IItem[] GetItems()
        {
            var items = new IItem[_capacity];

            for (var i = 0; i < _capacity; i++)
            {
                var slot = _slots[i];
                
                items[i] = slot.CheckLocked() ? null : slot.CheckAssigned() ? _slots[i] : null;
            }
            
            return items;
        }
    }
}