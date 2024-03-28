using System;
using UnityEngine;

namespace Oxygen
{
	[Serializable]
	public class BreakBoxFragment
	{
		[SerializeField] private int _count;

		[SerializeField] private Vector2 _impactSpeedRange;
		[SerializeField] private Vector2 _destroyTimeRange;

		[SerializeField] private GameObject _gameObject;

		public int GetCount()
		{
			return _count;
		}

		public GameObject GetGameObject() 
		{
			return _gameObject;
		}

		public Vector2 GetImpactSpeedRange()
		{
			return _impactSpeedRange;
		}

		public Vector2 GetDestroyTimeRange()
		{
			return _destroyTimeRange;
		}
	}

	public class BreakBox : BaseDestructive
	{
		[SerializeField] private BreakBoxFragment[] _fragments;

		private Transform _transform;

		protected override void Start()
		{
			base.Start();

			_transform = transform;
		}

		protected override void OnKilled(GameObject caller)
		{
			foreach(var fragment in _fragments) 
			{
				for(var i = 0; i < fragment.GetCount(); i++)
				{
					var randomPosition = UnityEngine.Random.onUnitSphere;

					var position = _transform.position;
					var spawnPosition = position + randomPosition / 2;
					var spawnRotation = Quaternion.LookRotation(randomPosition);

					var directionToSpawnPosition = (spawnPosition - position).normalized;

					var directionFromCaller = (spawnPosition - caller.transform.position).normalized;
					var fragmentRigidbody = Instantiate(fragment.GetGameObject(), spawnPosition, spawnRotation).GetComponent<Rigidbody>();

					var speed = UnityEngine.Random.Range(fragment.GetImpactSpeedRange().x, fragment.GetImpactSpeedRange().y);
					var destroyTime = UnityEngine.Random.Range(fragment.GetDestroyTimeRange().x, fragment.GetDestroyTimeRange().y);

					fragmentRigidbody.AddForce(directionToSpawnPosition + directionFromCaller * speed, ForceMode.Impulse);

					Destroy(fragmentRigidbody.gameObject, destroyTime);
				}
			}
		}

		protected Transform GetTransform()
		{
			return _transform;
		}
	}
}