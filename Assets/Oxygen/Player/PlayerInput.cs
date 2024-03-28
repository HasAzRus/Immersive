using System;
using UnityEngine;

namespace Oxygen
{
    public enum InputMode
    {
        Game,
        UI,
    }
    
    public class PlayerInput : Behaviour
    {
        public event Action<InputMode> ModeChanged;

        private InputMode _previousMode;
        private InputMode _mode;
        
        private bool _isEnabled;

        private Player _player;
        
        public void Construct(Player player)
        {
            OnConstruct(player);

            _player = player;
        }

        protected virtual void OnConstruct(Player player)
        {
            
        }
        
        protected virtual void OnUserInput()
        {
            
        }

        protected virtual void OnPlayerInput()
        {
            switch (_mode)
            {
                case InputMode.Game:
                    OnGameInput();
                    break;
                case InputMode.UI:
                    OnUserInterfaceInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnGameInput()
        {
            
        }

        protected virtual void OnUserInterfaceInput()
        {
            
        }

        protected override void Update()
        {
            base.Update();

            OnUserInput();
            
            if (!_isEnabled)
            {
                return;
            }
            
            OnPlayerInput();
        }

        protected Player GetPlayer()
        {
            return _player;
        }

        public void SetMode(InputMode value)
        {
            var isGame = value == InputMode.Game;

            SetCursorVisible(!isGame);
            SetCursorLockState(isGame);

            _previousMode = _mode;
            _mode = value;

            ModeChanged?.Invoke(value);
        }

        public void SetCursorVisible(bool value)
        {
            Cursor.visible = value;
        }

        public void SetCursorLockState(bool value)
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void SetEnabled(bool value)
        {
            _isEnabled = value;
        }

        public InputMode GetMode()
        {
            return _mode;
        }

        public InputMode GetPreviousMode()
        {
            return _previousMode;
        }

        public bool GetEnabled()
        {
            return _isEnabled;
        }
    }
}