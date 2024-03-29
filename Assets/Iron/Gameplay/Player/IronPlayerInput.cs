using Oxygen;
using UnityEngine;

namespace Iron
{
    public enum CrouchInputMode
    {
        Single,
        Press
    }
    
    public class IronPlayerInput : FirstPersonInput
    {
        [SerializeField] private CrouchInputMode _crouchInputMode;

        private bool _isCrouchPress;
        private bool _isRunningInput;
        
        private bool _isInventory;
        
        protected override void OnPlayerInput()
        {
            base.OnPlayerInput();

            if (GetPlayer() is not IronPlayer ironPlayer)
            {
                return;
            }

            if (Input.GetButtonDown("Inventory"))
            {
                _isInventory = !_isInventory;

                if (_isInventory)
                {
                    ironPlayer.OpenInventory();
                }
                else
                {
                    ironPlayer.CloseInventory();
                }
            }
        }

        protected override void OnGameInput()
        {
            base.OnGameInput();
            
            if (GetPlayer() is not IronPlayer ironPlayer)
            {
                return;
            }

            if (ironPlayer.Motor is not IronPlayerMotor ironPlayerMotor)
            {
                return;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (ironPlayer.ReadableStamina.IsEnough)
                {
                    ironPlayerMotor.Jump();
                }
            }

            if (Input.GetButtonDown("Crouch"))
            {
                if (_crouchInputMode == CrouchInputMode.Single)
                {
                    var isCrouching = ironPlayerMotor.CheckCrouching();

                    if (!isCrouching)
                    {
                        ironPlayerMotor.Crouch();
                    }
                    else
                    {
                        ironPlayerMotor.Uncrouch();
                    }
                }
                else if (_crouchInputMode == CrouchInputMode.Press)
                {
                    ironPlayerMotor.Crouch();

                    _isCrouchPress = true;
                }
            }

            if (Input.GetButtonUp("Crouch"))
            {
                if (_crouchInputMode == CrouchInputMode.Press)
                {
                    _isCrouchPress = false;
                }
            }

            if (_crouchInputMode == CrouchInputMode.Press)
            {
                if (!_isCrouchPress)
                {
                    if (ironPlayerMotor.CheckCrouching())
                    {
                        ironPlayerMotor.Uncrouch();
                    }
                }
            }
            
            _isRunningInput = Input.GetButton("Sprint") && !ironPlayerMotor.CheckCrouching() &&
                              ironPlayer.ReadableStamina.IsEnough;

            if (_isRunningInput)
            {
                ironPlayerMotor.StartRunning();
            }
            else
            {
                ironPlayerMotor.StopRunning();
            }
        }
    }
}