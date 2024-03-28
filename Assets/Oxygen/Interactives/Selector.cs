using UnityEngine;

namespace Oxygen
{
    public enum SelectorMode
    {
        Sequentially,
        PinkPong
    }
    
    public class Selector : BaseInteractive
    {
        [SerializeField] private SelectorMode _mode;
        
        [SerializeField] private int _maxState;

        private int _direction;
        private int _state;

        private void SetState(int value)
        {
            _state = value;
            
            OnStateChanged(value);
        }

        protected virtual void OnStateChanged(int value)
        {
            
        }
        
        protected override bool OnInteract(Player player)
        {
            if (_direction == 1)
            {
                if (_state + 1 < _maxState)
                {
                    SetState(_state + 1);
                }
                else
                {
                    if (_mode == SelectorMode.Sequentially)
                    {
                        SetState(0);
                    }
                    else if (_mode == SelectorMode.PinkPong)
                    {
                        SetState(_state - 1);

                        _direction = -1;
                    }
                }
            }
            else if (_direction == -1)
            {
                if (_state - 1 >= 0)
                {
                    SetState(_state - 1);
                }
                else
                {
                    if (_mode == SelectorMode.PinkPong)
                    {
                        SetState(_state + 1);

                        _direction = 1;
                    }
                }
            }

            return true;
        }

        protected override void Start()
        {
            base.Start();

            _direction = 1;
        }

        public int GetMaxState()
        {
            return _maxState;
        }
    }
}