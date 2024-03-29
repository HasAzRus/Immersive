using System;
using UnityEngine;

namespace Oxygen
{
    [Serializable]
    public class FireImpact
    {
        [SerializeField] private Range _horizontal;
        [SerializeField] private Range _vertical;

        public Range Horizontal => _horizontal;
        public Range Vertical => _vertical;
    }

}