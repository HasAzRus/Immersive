using System;
using UnityEngine;

namespace Oxygen
{
    public interface IReadableEnergy : IReadableHealth
    {
        
    }

    public interface IWritableEnergy : IWritableHealth
    {
        
    }
    
    [Serializable]
    public class Energy : IWritableEnergy, IReadableEnergy
    {
        public event Action<float> ValueChanged;

        [SerializeField] private float _maxValue;
        
        public float Value { get; private set; }
        public float MaxValue => _maxValue;
        public bool IsEmpty { get; private set; }

        private void SetValue(float value)
        {
            Value = value;
            ValueChanged?.Invoke(value);
        }
        
        public void Apply(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Значение amount ниже нуля");
            }
            
            SetValue(Mathf.Clamp(Value, 1f, MaxValue));
        }

        public void Add(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Значение amount ниже нуля");
            }

            var value = Value + amount;

            if (value > 0)
            {
                IsEmpty = false;
            }

            SetValue(value < MaxValue ? value : MaxValue);
        }

        public void Remove(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Значение amount ниже нуля");
            }

            var value = Value - amount;

            if (value > 0)
            {
                SetValue(value);
            }
            else
            {
                IsEmpty = true;
                
                SetValue(0);
            }
        }

        public void Fill()
        {
            SetValue(MaxValue);
        }
    }
}