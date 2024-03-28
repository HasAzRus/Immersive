using System;
using UnityEngine;

namespace Oxygen
{
    [Serializable]
    public class KeyValue<T>
    {
        [SerializeField] private string _key;
        [SerializeField] private T _value;

        public string Key => _key;
        public T Value => _value;
    }
    
    public class Database<T> : ScriptableObject
    {
        [SerializeField] private KeyValue<T>[] _keyValues;

        public bool TryGetValue(string key, out T value)
        {
            value = default;

            foreach (var keyValue in _keyValues)
            {
                if (keyValue.Key != key)
                {
                    continue;
                }

                value = keyValue.Value;

                return true;
            }
            
            return false;
        }
    }
}