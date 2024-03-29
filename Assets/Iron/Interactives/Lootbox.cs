using System;
using UnityEngine;

namespace Iron
{
	[Serializable]
	public class Loot : Oxygen.Item
	{
		[SerializeField] [Range(0f, 1f)] private float _dropProbability;

		public float GetDropProbability()
		{
			return _dropProbability;
		}
	}

	public class Lootbox : BaseTimeGrabInteractive
	{
		[SerializeField] private Loot[] _loots;

		[SerializeField] private ItemDatabase _itemDatabase;

		private Transform _transform;

		protected override void OnTimeInteraction(IronPlayer ironPlayer)
		{
			foreach (var loot in _loots)
			{
				var randomProbality = UnityEngine.Random.Range(0f, 1f);

				if (loot.GetDropProbability() < randomProbality)
				{
					Debug.Log(
						$"Вероятность выпадения предмета({loot.Name}, {loot.GetDropProbability()}) ниже текущей ({randomProbality})");

					continue;
				}

				Debug.Log($"Выпал предмет: {loot.Name}");

				var _isFloorExists = !Physics.Raycast(_transform.position, -_transform.up, out RaycastHit hit);

				if (!_itemDatabase.TryGetValue(loot.Name, out var item))
				{
					throw new Exception("Отсутствует предмет с таким наименованием");
				}

				var randomCount = UnityEngine.Random.Range(1, loot.Count + 1);

				for (var i = 0; i < randomCount; i++)
				{
					Instantiate(item.Prefab, _isFloorExists ? hit.point : _transform.position, Quaternion.identity);
				}
			}
			
			Destroy(gameObject);
		}

		protected override void Start()
		{
			base.Start();

			_transform = transform;
		}
	}
}