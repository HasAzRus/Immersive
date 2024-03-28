using System;
using System.Collections.Generic;
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
        bool IsAssigned { get; }
        bool IsLocked { get; }
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

        public bool IsAssigned { get; private set; }
        public bool IsLocked { get; private set; }

        public void Assign(string name)
        {
            Name = name;
            IsAssigned = true;
        }

        public void Clear()
        {
            Name = string.Empty;
            IsAssigned = false;
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
        }

        public void SetLocked(bool value)
        {
            IsLocked = value;
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
                
                _slots[i] = slot;
            }
        }

        public bool Place(string name, int count)
        {
            Slot firstFreeSlot = null;
            
            foreach (var slot in _slots)
            {
                if (slot.IsLocked)
                {
                    continue;
                }
                
                if (slot.IsAssigned)
                {
                    if (slot.Name != name)
                    {
                        continue;
                    }
                    
                    slot.Add(count);

                    Debug.Log($"Был добавлен предмет {name} в кол-ве {count}");
                    Placed?.Invoke(name, count);

                    return true;
                }

                firstFreeSlot ??= slot;
            }

            if (firstFreeSlot == null)
            {
                return false;
            }
            
            firstFreeSlot.Assign(name);
            firstFreeSlot.Add(count);

            Placed?.Invoke(name, count);
            
            return true;
        }

        public bool Place(IItem item)
        {
            return Place(item.Name, item.Count);
        }

        public bool Remove(string name)
        {
            foreach (var slot in _slots)
            {
                if (slot.IsLocked)
                {
                    continue;
                }
                
                if (!slot.IsAssigned)
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
                if (slot.IsLocked)
                {
                    continue;
                }
                
                if (!slot.IsAssigned)
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

        public bool CheckExists(string name)
        {
            foreach (var slot in _slots)
            {
                if (slot.IsLocked)
                {
                    continue;
                }
                
                if (!slot.IsAssigned)
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

        public bool CheckExists(string name, int count)
        {
            foreach (var slot in _slots)
            {
                if (slot.IsLocked)
                {
                    continue;
                }
                
                if (!slot.IsAssigned)
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

        public bool CheckExists(IItem item)
        {
            return CheckExists(item.Name, item.Count);
        }

        public IItem[] GetItems()
        {
            var items = new List<IItem>();

            foreach (var slot in _slots)
            {
                if (slot.IsLocked)
                {
                    continue;
                }

                if (!slot.IsAssigned)
                {
                    continue;
                }
                
                items.Add(slot);
            }

            return items.ToArray();
        }
    }
}