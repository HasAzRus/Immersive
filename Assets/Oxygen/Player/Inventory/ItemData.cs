using System;
using Iron;
using UnityEngine;

namespace Oxygen
{
    [Serializable]
    public class ItemData
    {
        [SerializeField] private int _count;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Pickup _prefab;
        [SerializeField] private ItemInteractive _interactive;
        
        public int Count => _count;
        public Sprite Sprite => _sprite;
        public Pickup Prefab => _prefab;
        public ItemInteractive Interactive => _interactive;
    }
}