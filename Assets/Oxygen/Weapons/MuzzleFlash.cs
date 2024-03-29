using System;
using UnityEngine;

namespace Oxygen
{
    [Serializable]
    public class MuzzleFlash
    {
        [SerializeField] private float _maxTime;

        [SerializeField] private Light _light;

        private bool _isFlashing;
        private float _time;

        public void Update()
        {
            if (_isFlashing)
            {
                if (_time < _maxTime)
                {
                    _time += Time.deltaTime;
                }
                else
                {
                    Clear();
                }

                _light.intensity = 1.0f - _time / _maxTime;
            }
        }

        public void Flash()
        {
            _isFlashing = true;

            _light.enabled = true;
        }

        public void Clear()
        {
            _isFlashing = false;
            _time = 0f;

            _light.enabled = false;
        }
    }
}