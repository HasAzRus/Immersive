using Oxygen;
using UnityEngine;

namespace Iron
{
    public abstract class BaseTimeGrabInteractive : BaseConstraintGrabInteractive
    {
        [SerializeField] private float _maxTime;
        [SerializeField] private bool _onlyOnce;

        private bool _isFinished;

        protected abstract void OnTimeInteraction(IronPlayer ironPlayer);

        protected virtual void OnTimeInteractionFailed(IronPlayer ironPlayer)
        {
            
        }

        protected override void OnStopInteraction(Player player)
        {
            base.OnStopInteraction(player);

            if (player is not IronPlayer ironPlayer)
            {
                return;
            }

            if (_isFinished)
            {
                OnTimeInteraction(ironPlayer);

                if (_onlyOnce)
                {
                    SetEnabled(false);
                }
                
                return;
            }
            
            OnTimeInteractionFailed(ironPlayer);
        }

        public void StopTimeInteraction()
        {
            _isFinished = true;
        }
        
        public float MaxTime => _maxTime;
    }
}