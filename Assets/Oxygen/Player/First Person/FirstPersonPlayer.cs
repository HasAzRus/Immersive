using System;
using UnityEngine;

namespace Oxygen
{
    [RequireComponent(typeof(FirstPersonInput))]
    [RequireComponent(typeof(PlayerCameraInteraction))]
    [RequireComponent(typeof(PlayerPointer))]
    public class FirstPersonPlayer : Player
    {
        public event Action Interacting;
        public event Action InteractionStopped; 

        private FirstPersonMotor _motor;

        private PlayerInteraction _interaction;
        private PlayerPointer _pointer;

        protected override void OnGameBeginned()
        {
            base.OnGameBeginned();
            
            GetInput().SetEnabled(true);
            GetInput().SetMode(InputMode.Game);

            GetInteraction().IsEnabled = true;
            GetPointer().SetEnabled(true);

            if (GetCamera() is not FirstPersonCamera firstPersonCamera)
            {
                return;
            }
            
            firstPersonCamera.SetMouseLookEnabled(true);
        }

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            
            _motor = GetComponent<FirstPersonMotor>();

            _interaction = GetComponent<PlayerInteraction>();
            _pointer = GetComponent<PlayerPointer>();
            
            _pointer.Construct(this);
        }

        protected virtual void OnInteracting()
        {
            
        }

        protected virtual void OnInteractionStopped()
        {
            
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _pointer.TryClear();
        }

        public bool TryInteract()
        {
            if (!_interaction.TryInteract(this))
            {
                return false;
            }

            if (_interaction.CurrentInteractive is BaseConstraintGrabInteractive constraintGrabInteractive)
            {
                if (GetInput() is FirstPersonInput firstPersonInput)
                {
                    firstPersonInput.SetConstraint(constraintGrabInteractive.Constraint);
                }
            }

            OnInteracting();
            Interacting?.Invoke();

            return true;
        }

        public bool TryStopInteraction()
        {
            if (!_interaction.TryStopInteraction(this))
            {
                return false;
            }
            
            if (GetInput() is FirstPersonInput firstPersonInput)
            {
                firstPersonInput.SetConstraint(FirstPersonInputConstraint.None);
            }
            
            OnInteractionStopped();
            InteractionStopped?.Invoke();

            return true;
        }

        public void MoveForward(float value)
        {
            _motor.MoveForward(value);
        }

        public void MoveRight(float value)
        {
            _motor.MoveRight(value);
        }

        public void LookAt(float value)
        {
            if (GetCamera() is not FirstPersonCamera firstPersonCamera)
            {
                return;
            }
            
            firstPersonCamera.LookAt(value);
        }

        public void Turn(float value)
        {
            if (GetCamera() is not FirstPersonCamera firstPersonCamera)
            {
                return;
            }
            
            firstPersonCamera.Turn(value);
        }

        public FirstPersonMotor GetMotor()
        {
            return _motor;
        }

        public PlayerInteraction GetInteraction()
        {
            return _interaction;
        }

        public PlayerPointer GetPointer()
        {
            return _pointer;
        }
    }
}