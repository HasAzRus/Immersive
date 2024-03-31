using System;
using Oxygen;
using UnityEngine;

namespace Iron
{
    public interface IReadableStamina : IReadableHealth
    {
        float MinEnoughValue { get; }
        bool IsEnough { get; }
    }

    public interface IWritableStamina : IWritableHealth
    {
        
    }
    
    [Serializable]
    public class Stamina : IReadableStamina, IWritableStamina
    {
        public event Action<float> ValueChanged;

        [SerializeField] private float _maxValue;
        [SerializeField] private float _minEnoughValue;

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
            
            SetValue(Mathf.Clamp(amount, 1f, MaxValue));
        }

        public void Add(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Значение amount ниже нуля");
            }

            var value = Value + amount;

            IsEmpty = value > 0;
            IsEnough = value >= _minEnoughValue;
            
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
                IsEnough = false;
                
                SetValue(0);
            }
        }

        public void Fill()
        {
            SetValue(MaxValue);
        }
        
        public float Value { get; private set; }
        
        public float MaxValue => _maxValue;
        public float MinEnoughValue => _minEnoughValue;
        
        public bool IsEmpty { get; private set; }
        public bool IsEnough { get; private set; }
    }
}