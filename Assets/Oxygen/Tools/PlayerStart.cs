using UnityEngine;

namespace Oxygen
{
	public class PlayerStart : Behaviour
	{
		private void OnDrawGizmos()
		{
			var cachedTransform = transform;
			var position = cachedTransform.position;
			
			Gizmos.DrawLine(position, position + cachedTransform.forward);

			Gizmos.matrix = cachedTransform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Preferences.PlayerStartSize);
		}
	}
}