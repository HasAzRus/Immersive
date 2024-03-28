using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oxygen
{
    [Serializable]
    public class GameData : ISerializationCallbackReceiver
    {
        public List<string> Keys;
        public List<object> Values;

        private Dictionary<string, object> _dataMap;

        public GameData()
        {
            Keys = new List<string>();
            Values = new List<object>();

            _dataMap = new Dictionary<string, object>();
        }

        public void AddValue<T>(string key, T value)
        {
            _dataMap[key] = value;
        }

        public T GetValue<T>(string key, T valueByDefault)
        {
            if (_dataMap.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            AddValue(key, valueByDefault);

            return valueByDefault;
        }

        public void OnBeforeSerialize()
        {
            Keys.Clear();
            Values.Clear();

            foreach (var data in _dataMap)
            {
                Keys.Add(data.Key);
                Values.Add(data.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _dataMap = new Dictionary<string, object>();

            for (var i = 0; i < Mathf.Min(Keys.Count, Values.Count); i++)
            {
                _dataMap.Add(Keys[i], Values[i]);
            }
        }
    }
}