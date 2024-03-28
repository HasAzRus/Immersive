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

        protected override void OnGameBeginned()
        {
            base.OnGameBeginned();
            
            Input.SetEnabled(true);
            Input.SetMode(InputMode.Game);

            Interaction.IsEnabled = true;
            Pointer.SetEnabled(true);

            if (Camera is not FirstPersonCamera firstPersonCamera)
            {
                return;
            }
            
            firstPersonCamera.SetMouseLookEnabled(true);
        }

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            
            Motor = GetComponent<FirstPersonMotor>();
            Interaction = GetComponent<PlayerInteraction>();
            Pointer = GetComponent<PlayerPointer>();
            
            Pointer.Construct(this);
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

            Pointer.TryClear();
        }

        public bool TryInteract()
        {
            if (!Interaction.TryInteract(this))
            {
                return false;
            }

            if (Interaction.CurrentInteractive is BaseConstraintGrabInteractive constraintGrabInteractive)
            {
                if (Input is FirstPersonInput firstPersonInput)
                {
                    firstPersonInput.Constraint = constraintGrabInteractive.Constraint;
                }
            }

            OnInteracting();
            Interacting?.Invoke();

            return true;
        }

        public bool TryStopInteraction()
        {
            if (!Interaction.TryStopInteraction(this))
            {
                return false;
            }
            
            if (Input is FirstPersonInput firstPersonInput)
            {
                firstPersonInput.Constraint = FirstPersonInputConstraint.None;
            }
            
            OnInteractionStopped();
            InteractionStopped?.Invoke();

            return true;
        }

        public void MoveForward(float value)
        {
            Motor.MoveForward(value);
        }

        public void MoveRight(float value)
        {
            Motor.MoveRight(value);
        }

        public void LookAt(float value)
        {
            if (Camera is not FirstPersonCamera firstPersonCamera)
            {
                return;
            }
            
            firstPersonCamera.LookAt(value);
        }

        public void Turn(float value)
        {
            if (Camera is not FirstPersonCamera firstPersonCamera)
            {
                return;
            }
            
            firstPersonCamera.Turn(value);
        }

        public FirstPersonMotor Motor { get; private set; }
        public PlayerInteraction Interaction { get; private set; }
        public PlayerPointer Pointer { get; private set; }
    }
}