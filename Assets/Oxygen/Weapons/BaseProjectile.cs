using UnityEngine;

namespace Oxygen
{
	public abstract class BaseProjectile : Behaviour
	{
		[SerializeField] private float _maxDestroyTime;
		[SerializeField] private bool _deactiveGameObjectAfterDestroy;

		private float _destroyTime;

		private float _damage;
		private GameObject _ownerGameObject;

		public void Construct(GameObject ownerGameObject, float damage)
		{
			_ownerGameObject = ownerGameObject;
			_damage = damage;

			_destroyTime = 0f;

			OnConstruct(ownerGameObject, damage);
		}

		protected virtual void OnConstruct(GameObject ownerGameObject, float damage)
		{

		}

		protected abstract void OnLaunch(Vector3 direction);

		protected override void Update()
		{
			base.Update();

			if(_destroyTime < _maxDestroyTime)
			{
				_destroyTime += Time.deltaTime;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}

		public void Launch(Vector3 direction)
		{
			OnLaunch(direction);

			if (!_deactiveGameObjectAfterDestroy)
			{
				Destroy(gameObject, _maxDestroyTime);
			}
		}

		public GameObject GetOwner()
		{
			return _ownerGameObject;
		}

		public float GetDamage()
		{
			return _damage;
		}

		public void Destroy()
		{
			if (_deactiveGameObjectAfterDestroy)
			{
				gameObject.SetActive(false);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}