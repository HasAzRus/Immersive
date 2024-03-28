using System;
using UnityEngine;

namespace Oxygen
{
    public enum TouchTriggerMode
    {
        Enter,
        Exit,
        Both
    }
    
    [RequireComponent(typeof(BoxCollider))]
    public class TouchTrigger : Behaviour
    {
        public event Action<Player> Triggered;
        public event Action<Player> Untriggered;
        
        [SerializeField] private TouchTriggerMode _mode;
        [SerializeField] private bool _onlyOnce;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Preferences.TouchTriggerColor;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            var checkOnlyOnce = true;

            if (_mode != TouchTriggerMode.Enter)
            {
                if (_mode != TouchTriggerMode.Both)
                {
                    return;
                }

                checkOnlyOnce = false;
            }
            
            if (!other.TryGetComponent(out Player player))
            {
                return;
            }
            
            Triggered?.Invoke(player);

            if (!checkOnlyOnce)
            {
                return;
            }
            
            if (_onlyOnce)
            {
                Destroy(gameObject);
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            if (_mode != TouchTriggerMode.Exit)
            {
                if (_mode != TouchTriggerMode.Both)
                {
                    return;
                }
            }
            
            if (!other.TryGetComponent(out Player player))
            {
                return;
            }
            
            Untriggered?.Invoke(player);
            
            if (_onlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}