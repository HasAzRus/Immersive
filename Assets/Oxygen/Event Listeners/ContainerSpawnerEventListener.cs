using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
	public class ContainerSpawnerEventListener : Behaviour
	{
		[SerializeField] private ContainerSpawner _target;

		[SerializeField] private UnityEvent<GameObject> _onRandomOneSpawnedEvent;
		[SerializeField] private UnityEvent<GameObject[]> _onAllSpawnedEvent;

		private void OnRandomOneSpawned(GameObject obj)
		{
			_onRandomOneSpawnedEvent.Invoke(obj);
		}

		private void OnAllSpawned(GameObject[] objs)
		{
			_onAllSpawnedEvent.Invoke(objs);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_target.RandomOneSpawned += OnRandomOneSpawned;
			_target.AllSpawned += OnAllSpawned;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_target.RandomOneSpawned -= OnRandomOneSpawned;
			_target.AllSpawned -= OnAllSpawned;
		}
	}
}