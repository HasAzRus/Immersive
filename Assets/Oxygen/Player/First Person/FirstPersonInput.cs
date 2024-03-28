using UnityEngine;

namespace Oxygen
{
    public enum FirstPersonInputConstraint
    {
        None,
        OnlyMovement,
        OnlyRotation,
        All
    }
    
    public class FirstPersonInput : PlayerInput
    {
        [SerializeField] private bool _allowToggleToFreeCamera;
        [SerializeField] private float _sensitive;

        protected override void OnGameInput()
        {
            base.OnGameInput();

            if (GetPlayer() is not FirstPersonPlayer firstPersonPlayer)
            {
                return;
            }
            
            var disableMovement = Constraint is FirstPersonInputConstraint.All or FirstPersonInputConstraint.OnlyMovement;
            
            if (!disableMovement)
            {
                firstPersonPlayer.MoveForward(Input.GetAxis("Vertical"));
                firstPersonPlayer.MoveRight(Input.GetAxis("Horizontal"));
            }

            if (_allowToggleToFreeCamera)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    var isGhostMode = firstPersonPlayer.Motor.GetMode() == FirstPersonMotorMode.Spectator;

                    firstPersonPlayer.Motor
                        .SetMode(isGhostMode ? FirstPersonMotorMode.Default : FirstPersonMotorMode.Spectator);
                }
            }
            
            var disableRotation = Constraint is FirstPersonInputConstraint.All or FirstPersonInputConstraint.OnlyRotation;
            
            if (!disableRotation)
            {
                firstPersonPlayer.LookAt(-Input.GetAxis("Mouse Y") * _sensitive);
                firstPersonPlayer.Turn(Input.GetAxis("Mouse X") * _sensitive);
            }

            if (Input.GetButtonDown("Interact"))
            { 
                firstPersonPlayer.TryInteract();
            }

            if (Input.GetButtonUp("Interact"))
            {
                firstPersonPlayer.TryStopInteraction();
            }
        }

        public FirstPersonInputConstraint Constraint { get; set; }
    }
}