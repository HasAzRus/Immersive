using UnityEngine;

namespace Oxygen
{
    public class PlayerCameraInteraction : PlayerInteraction
    {
        [SerializeField] private float _distance;
        [SerializeField] private LayerMask _layerMask;
        
        [SerializeField] private Camera _camera;
        
        private Transform _cameraTransform;
        
        protected override void Start()
        {
            base.Start();

            _cameraTransform = _camera.transform;
        }
        
        protected override bool TryGetInteractive(out IInteractive interactive)
        {
            interactive = null;
            
            var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            
            return World.Trace(ray, _distance, _layerMask,
                out var hitInfo) && hitInfo.collider.TryGetComponent(out interactive);
        }
    }
}