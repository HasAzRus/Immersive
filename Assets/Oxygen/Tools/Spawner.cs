using System;
using UnityEngine;

namespace Oxygen
{
	public class Spawner : Behaviour
	{
		public event Action<GameObject> Spawned;

		[SerializeField] private GameObject _original;

		[SerializeField] private Transform _target;

		private Transform _transform;

		protected override void Start()
		{
			base.Start();

			_transform = transform;
		}

		public void Spawn()
		{
			var spawnedGameObject = Instantiate(_original, _transform.position, Quaternion.identity);

			if(_target != null)
			{
				spawnedGameObject.transform.SetPositionAndRotation(_target.position, _target.rotation);
			}

			Spawned?.Invoke(spawnedGameObject);
		}
	}
}