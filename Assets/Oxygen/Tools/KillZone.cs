using System;
using UnityEngine;

namespace Oxygen
{
    [RequireComponent(typeof(BoxCollider))]
    public class KillZone : Behaviour
    {
        [SerializeField] private bool _allowDestroyGameObject;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Preferences.KillZoneColor;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (_allowDestroyGameObject)
            {
                Destroy(other.gameObject);

                return;
            }

            if (!other.TryGetComponent(out IKillable killable))
            {
                return;
            }
            
            killable.Kill(gameObject);
        }
    }
}