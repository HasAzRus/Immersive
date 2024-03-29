using System;
using UnityEngine;

namespace Oxygen
{
    public interface IReadableHealth
    {
        event Action<float> ValueChanged;
        
        float Value { get; }
        float MaxValue { get; }
        bool IsEmpty { get; }
    }

    public interface IWritableHealth : IReadableHealth
    {
        void Apply(float amount);
        void Add(float amount);
        void Remove(float amount);
        void Fill();
    }
    
    [Serializable]
    public class Health : IWritableHealth
    {
        public event Action<float> ValueChanged;
        
        [SerializeField] private float _maxValue;

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
        
        public float Value { get; private set; }
        public float MaxValue => _maxValue;
        public bool IsEmpty { get; private set; }
    }
}