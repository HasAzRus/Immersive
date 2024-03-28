using UnityEngine;

namespace Oxygen
{
	public abstract class BaseProjectileLauncher : BaseFirearmWeapon
	{
		[Header("Projectile Launcher")]

		[SerializeField] private float _speed;

		[SerializeField] private bool _allowMultiplier;
		[SerializeField] private bool _isInRow;

		[SerializeField] private float[] _offset;

		[SerializeField] private BaseProjectile _defaultProjectile;

		[SerializeField] private bool _usePoolerProjectile;

		[SerializeField] private string _poolerName;
		[SerializeField] private string _poolerProjectileName;

		private int _index;

		private void Launch(BaseProjectile projectile, Vector3 direction)
		{
			projectile.Construct(GetOwner(), GetDamage() * GetDamageMultiplier());
			projectile.Launch(_speed * direction);
		}
		
		private BaseProjectile Create(Vector3 position, Quaternion rotation)
		{
			switch (_usePoolerProjectile)
			{
				case true:
				{
					if (!Pooler.TryGetPooler(_poolerName, out var pooler))
					{
						return null;
					}
				
					var projectileTransform = pooler.GetGameObject(_poolerProjectileName).transform;

					projectileTransform.SetPositionAndRotation(position, rotation);

					return projectileTransform.GetComponent<BaseProjectile>();
				}
				default:
					return Instantiate(_defaultProjectile, position, rotation);
			}
		}

		private BaseProjectile CreateByOffset(int index, Vector3 offsetAxis, Vector3 position, Quaternion rotation)
		{
			return Create(position + offsetAxis * _offset[index], rotation);
		}

		protected override void OnShoot()
		{
			base.OnShoot();

			var traceTransform = GetTrunkTransform();

			if (CheckAllowOwnerDirection())
			{
				traceTransform = GetOwnerTransform();
			}
			
			var position = traceTransform.position;

			if (_allowMultiplier)
			{
				if(_isInRow)
				{
					Launch(CreateByOffset(_index, traceTransform.right, position, traceTransform.rotation), traceTransform.forward);

					if(_index + 1 < _offset.Length) 
					{
						_index += 1;
					}
					else
					{
						_index = 0;
					}
				}
				else
				{
					for (var i = 0; i < _offset.Length; i++)
					{
						Launch(CreateByOffset(i, traceTransform.right, position, traceTransform.rotation), traceTransform.forward);
					}
				}
			}
			else
			{
				Launch(Create(position, traceTransform.rotation), traceTransform.forward);
			}
		}
	}
}