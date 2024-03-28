using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oxygen
{
	[Serializable]
	public class PoolerObject
	{
		[SerializeField] private string _name;
		[SerializeField] private int _count;

		[SerializeField] private GameObject _gameoObject;

		public string GetName()
		{
			return _name;
		}

		public int GetCount()
		{
			return _count;
		}

		public GameObject GetGameObject() 
		{
			return _gameoObject;
		}
	}

	public class Pooler : Behaviour
	{
		private static List<Pooler> _poolers;

		[SerializeField] private string _name;
		[SerializeField] private PoolerObject[] _objects;

		private List<GameObject> _pooledGameObjects;

		protected override void Awake()
		{
			base.Awake();

			if(_poolers == null)
			{
				_poolers = new List<Pooler>();
			}
			
			_poolers.Add(this);

			_pooledGameObjects = new List<GameObject>();
		}

		protected override void Start()
		{
			base.Start();

			foreach(var objectToPool in _objects) 
			{
				for(var i = 0; i < objectToPool.GetCount(); i++)
				{
					var pooledGameObject = Instantiate(objectToPool.GetGameObject());
					
					pooledGameObject.name = objectToPool.GetName();
					pooledGameObject.SetActive(false);

					_pooledGameObjects.Add(pooledGameObject);
				}
			}
		}

		public GameObject GetGameObject(string name) 
		{
			foreach (var pooledGameObject in _pooledGameObjects)
			{
				if (pooledGameObject.activeInHierarchy)
				{
					continue;
				}

				if (pooledGameObject.name != name)
				{
					continue;
				}

				pooledGameObject.SetActive(true);

				return pooledGameObject;
			}

			return null;
		}

		public static bool TryGetPooler(string name, out Pooler pooler)
		{
			pooler = null;
			
			foreach (var iPooler in _poolers)
			{
				if (iPooler._name != name)
				{
					continue;
				}

				pooler = iPooler;

				return true;
			}
			
			return false;
		}
	}
}