using UnityEngine;

namespace Oxygen
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class BaseTouchTrigger : Behaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Preferences.TouchTriggerColor;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }

        protected abstract void OnTriggered(Player player);

        protected virtual void OnUntriggered(Player player)
        {
            
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if (!other.TryGetComponent(out Player player))
            {
                return;
            }
            
            OnTriggered(player);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            if (!other.TryGetComponent(out Player player))
            {
                return;
            }
            
            OnUntriggered(player);
        }
    }
}