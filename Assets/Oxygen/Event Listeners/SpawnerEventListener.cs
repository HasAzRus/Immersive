using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
	public class SpawnerEventListener : Behaviour
	{
		[SerializeField] private Spawner _target;

		[SerializeField] private UnityEvent<GameObject> _onSpawnedEvent;

		private void OnSpawned(GameObject obj)
		{
			_onSpawnedEvent.Invoke(obj);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_target.Spawned += OnSpawned;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_target.Spawned -= OnSpawned;
		}
	}
}