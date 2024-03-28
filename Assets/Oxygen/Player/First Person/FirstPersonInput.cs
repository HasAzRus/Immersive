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

        private FirstPersonInputConstraint _constraint;
        
        protected override void OnGameInput()
        {
            base.OnGameInput();

            if (GetPlayer() is not FirstPersonPlayer firstPersonPlayer)
            {
                return;
            }
            
            var disableMovement = _constraint is FirstPersonInputConstraint.All or FirstPersonInputConstraint.OnlyMovement;
            
            if (!disableMovement)
            {
                firstPersonPlayer.MoveForward(Input.GetAxis("Vertical"));
                firstPersonPlayer.MoveRight(Input.GetAxis("Horizontal"));
            }

            if (_allowToggleToFreeCamera)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    var isGhostMode = firstPersonPlayer.GetMotor().GetMode() == FirstPersonMotorMode.Spectator;

                    firstPersonPlayer.GetMotor()
                        .SetMode(isGhostMode ? FirstPersonMotorMode.Default : FirstPersonMotorMode.Spectator);
                }
            }
            
            var disableRotation = _constraint is FirstPersonInputConstraint.All or FirstPersonInputConstraint.OnlyRotation;
            
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

        public FirstPersonInputConstraint GetConstraint()
        {
            return _constraint;
        }
        
        public void SetConstraint(FirstPersonInputConstraint value)
        {
            _constraint = value;
        }
    }
}