using Iron;
using UnityEngine;

namespace Oxygen
{
    public class BaseIronFirearmWeapon : BaseTraceFirearmWeapon
    {
        [SerializeField] private float _aimFieldOfView;
        [SerializeField] private string _ammoItemName;

        private IronPlayerMotor _playerMotor;
        private FirstPersonCamera _playerCamera;
        
        protected override void OnConstruction(Character owner)
        {
            base.OnConstruction(owner);

            if (owner is not Player player)
            {
                return;
            }

            if (player is not IronPlayer ironPlayer)
            {
                return;
            }

            if (ironPlayer.Motor is IronPlayerMotor ironPlayerMotor)
            {
                _playerMotor = ironPlayerMotor;
            }

            if (ironPlayer.Camera is FirstPersonCamera firstPersonCamera)
            {
                _playerCamera = firstPersonCamera;
            }
        }

        protected override void OnImpacting(FireImpact impact)
        {
            base.OnImpacting(impact);
            
            _playerCamera.AddHorizontalForce(Random.Range(impact.Horizontal.Min, impact.Horizontal.Max));
            _playerCamera.AddVerticalForce(Random.Range(impact.Vertical.Min, impact.Vertical.Max));
        }

        protected override void OnAiming()
        {
            base.OnAiming();
            
            _playerCamera.SetSmoothFieldOfView(_aimFieldOfView, 2f);
            _playerMotor.EnableAimingSpeed();
        }

        protected override void OnAimStopped()
        {
            base.OnAimStopped();

            _playerCamera.SetSmoothFieldOfView(_playerCamera.DefaultFieldOfView, 1f);
            _playerMotor.DisableAimingSpeed();
        }

        public string AmmoItemName => _ammoItemName;
    }
}