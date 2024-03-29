using System;
using UnityEngine;

namespace Oxygen
{
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
}