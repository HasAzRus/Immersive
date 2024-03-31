using System;
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

        private bool _isFiring;
        private bool _isAltFiring;
        
        private bool _isInventory;

        private void ToggleInventory(IronPlayer ironPlayer)
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

        private static void Jump(IronPlayer ironPlayer, IronPlayerMotor motor)
        {
            if (ironPlayer.Stamina.IsEnough)
            {
                motor.Jump();
            }
        }
        
        private void ToggleCrouch(IronPlayerMotor motor)
        {
            var isCrouching = motor.CheckCrouching();

            if (!isCrouching)
            {
                motor.Crouch();
            }
            else
            {
                motor.Uncrouch();
            }
        }

        private void Crouch(IronPlayerMotor motor)
        {
            motor.Crouch();

            _isCrouchPress = true;
        }

        private void Uncrouch()
        {
            if (_crouchInputMode == CrouchInputMode.Press)
            {
                _isCrouchPress = false;
            }
        }

        private void CheckToUncrouch(IronPlayerMotor motor)
        {
            if (_crouchInputMode != CrouchInputMode.Press)
            {
                return;
            }

            if (_isCrouchPress)
            {
                return;
            }
            
            if (motor.CheckCrouching())
            {
                motor.Uncrouch();
            }
        }

        private void HandleSprintInput(IronPlayer ironPlayer, IronPlayerMotor motor)
        {
            _isRunningInput = Input.GetButton("Sprint") && !motor.CheckCrouching() &&
                              ironPlayer.Stamina.IsEnough;

            if (_isRunningInput)
            {
                motor.StartRunning();
            }
            else
            {
                motor.StopRunning();
            }
        }

        private static void ToggleFlashlight(IronPlayer ironPlayer)
        {
            if (ironPlayer.Flashlight.IsFlashlightEnabled)
            {
                ironPlayer.DisableFlashlight();
            }
            else
            {
                ironPlayer.EnableFlashlight();
            }
        }
        
        protected override void OnPlayerInput()
        {
            base.OnPlayerInput();

            if (GetPlayer() is not IronPlayer ironPlayer)
            {
                return;
            }

            if (Input.GetButtonDown("Inventory"))
            {
                ToggleInventory(ironPlayer);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                SaveLoad.Save(true);
            }
        }

        private void Reload(IronPlayer ironPlayer)
        {
            var currentWeapon = ironPlayer.WeaponManager.CurrentWeapon;

            if (currentWeapon is not BaseIronFirearmWeapon ironFirearmWeapon)
            {
                return;
            }
            
            var itemName = ironFirearmWeapon.AmmoItemName;
                    
            var needAmmo = ironFirearmWeapon.MaxAmmo - ironFirearmWeapon.Ammo;
            var existsAmmo = ironPlayer.CheckExists(itemName);
                    
            Debug.Log($"Need Ammo: {needAmmo}, exists ammo: {existsAmmo}");

            if (existsAmmo <= 0)
            {
                return;
            }
            
            if (needAmmo > existsAmmo)
            {
                if (ironPlayer.RemoveItem(itemName))
                {
                                
                    ironFirearmWeapon.Reload(existsAmmo);
                }
            }
            else
            {
                if (ironPlayer.RemoveItem(itemName, needAmmo))
                {
                    ironFirearmWeapon.Reload(needAmmo);
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
                Jump(ironPlayer, ironPlayerMotor);
            }

            if (Input.GetButtonDown("Crouch"))
            {
                switch (_crouchInputMode)
                {
                    case CrouchInputMode.Single:
                        ToggleCrouch(ironPlayerMotor);
                        break;
                    case CrouchInputMode.Press:
                        Crouch(ironPlayerMotor);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Input.GetButtonUp("Crouch"))
            {
                Uncrouch();
            }
            
            CheckToUncrouch(ironPlayerMotor);
            
            HandleSprintInput(ironPlayer, ironPlayerMotor);

            if (Input.GetButtonDown("Fire1"))
            {
                var currentWeapon = ironPlayer.WeaponManager.CurrentWeapon;

                if (currentWeapon == null)
                {
                    Debug.Log("Оружие не активно");
                }
                else
                {
                    if (!currentWeapon.Fire(0))
                    {
                        Debug.Log("Оружие не работает");
                    }
                    else
                    {
                        _isFiring = true;
                    }
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                var currentWeapon = ironPlayer.WeaponManager.CurrentWeapon;

                if (currentWeapon == null)
                {
                    Debug.Log("Оружие не активно");
                }
                else
                {
                    if (_isFiring)
                    {
                        _isFiring = false;
                        
                        currentWeapon.StopFire(0);
                    }
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                var currentWeapon = ironPlayer.WeaponManager.CurrentWeapon;

                if (currentWeapon == null)
                {
                    Debug.Log("Оружие не активно");
                }
                else
                {
                    if (!currentWeapon.Fire(1))
                    {
                        Debug.Log("Оружиие не работает");
                    }
                    else
                    {
                        _isAltFiring = true;
                    }
                }
            }

            if (Input.GetButtonUp("Fire2"))
            {
                var currentWeapon = ironPlayer.WeaponManager.CurrentWeapon;

                if (currentWeapon == null)
                {
                    Debug.Log("Оружие не активно");
                }
                else
                {
                    if (_isAltFiring)
                    {
                        _isAltFiring = false;
                       
                        currentWeapon.StopFire(1);
                    }
                }
            }

            if (Input.GetButtonDown("Reload"))
            {
                Reload(ironPlayer);
            }

            if (Input.GetButtonDown("Hide Weapon"))
            {
                ironPlayer.HideWeapon();
            }

            if (Input.GetButtonDown("Flashlight"))
            {
                ToggleFlashlight(ironPlayer);
            }
        }
    }
}