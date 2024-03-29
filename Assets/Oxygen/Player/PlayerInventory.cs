using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oxygen
{
    public class PlayerInventory : Behaviour, IWritableInventory
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