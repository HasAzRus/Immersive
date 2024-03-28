using System;
using UnityEngine;

namespace Oxygen
{
    public abstract class BaseGrabInteractive : BaseInteractive
    {
        private bool _isGrabbing;

        protected override bool OnInteract(Player player)
        {
            _isGrabbing = true;

            return true;
        }

        protected override void OnStopInteraction(Player player)
        {
            base.OnStopInteraction(player);
            
            _isGrabbing = false;
        }

        public bool CheckGrabbing()
        {
            return _isGrabbing;
        }
    }
}