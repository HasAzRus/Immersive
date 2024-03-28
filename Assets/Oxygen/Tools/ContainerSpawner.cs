using System;
using UnityEngine;

namespace Oxygen
{
	public class ContainerSpawner : Behaviour
	{
		public event Action<GameObject> RandomOneSpawned;
		public event Action<GameObject[]> AllSpawned;

		[SerializeField] private GameObject[] _originals;
		[SerializeField] private ContainerSpawnerMode _mode;

		private Transform _transform;

		protected override void Start()
		{
			base.Start();

			_transform = transform;
		}

		public void Spawn()
		{
			if(_mode == ContainerSpawnerMode.RandomOne)
			{
				var randomOneOriginal = _originals[UnityEngine.Random.Range(0, _originals.Length)];

				var spawnedGameObject = Instantiate(randomOneOriginal, _transform.position, Quaternion.identity);

				RandomOneSpawned?.Invoke(spawnedGameObject);
			}
			else if(_mode == ContainerSpawnerMode.All)
			{
				GameObject[] allSpawnedGameObjects = new GameObject[_originals.Length];

				for(var i = 0; i <  _originals.Length; i++)
				{
					var spawnedGameObject = Instantiate(_originals[i], _transform.position, Quaternion.identity);
					allSpawnedGameObjects[i] = spawnedGameObject;
				}

				AllSpawned?.Invoke(allSpawnedGameObjects);
			}
		}
	}
}