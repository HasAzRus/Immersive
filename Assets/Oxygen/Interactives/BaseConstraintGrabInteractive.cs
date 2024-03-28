using UnityEngine;

namespace Oxygen
{
    public abstract class BaseConstraintGrabInteractive : BaseGrabInteractive
    {
        [SerializeField] private FirstPersonInputConstraint _constraint;

        public FirstPersonInputConstraint Constraint => _constraint;
    }
}