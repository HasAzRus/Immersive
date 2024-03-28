using UnityEngine;

namespace Oxygen
{
    public class Flasher : Behaviour
    {
        [SerializeField] private float _maxTime;

        [SerializeField] private bool _isAlways;
        [SerializeField] private bool _allowFlashingInStart;
        
        [SerializeField] private Color _color;

        private float _time;

        private bool _isFlashing;

        protected override void Start()
        {
            base.Start();

            if (_allowFlashingInStart)
            {
                Flash();
            }
        }

        protected override void Update() 
        {
            if (_isFlashing) 
            {
                if (_time < _maxTime) 
                {
                    _time += Time.deltaTime;
                }
                else
                {
                    _time -= _maxTime;

                    if (!_isAlways)
                    {
                        _isFlashing = false;
                    }
                }
                
                _color.a = Mathf.PingPong(_time / _maxTime, 0.5f);
            }
        }

        public void Flash(bool force = false)
        {
            if(_isFlashing && !force)
            {
                return;
            }

            _time = 0f;

            _isFlashing = true;
        }

        public void StopFlash()
        {
            _time = 0f;
            
            _isFlashing = false;
        }

        public Color GetColor()
        {
            return _color;
        }
    }
}