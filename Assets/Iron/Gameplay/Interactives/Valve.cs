using Oxygen;
using UnityEngine;

namespace Iron
{
    public class Valve : BaseGrabInteractive
    {
        [SerializeField] private Rotator _rotator;
        [SerializeField] private bool _onlyOnce;

        private void OnRotated()
        {
            if (_onlyOnce)
            {
                _rotator.enabled = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _rotator.Rotated += OnRotated;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _rotator.Rotated -= OnRotated;
        }

        protected override bool OnInteract(Player player)
        {
            _rotator.StartRotation(false);
            
            return base.OnInteract(player);
        }

        protected override void OnStopInteraction(Player player)
        {
            base.OnStopInteraction(player);

            _rotator.StartRotation(true);
        }
    }
}