using System;

namespace Oxygen
{
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
}