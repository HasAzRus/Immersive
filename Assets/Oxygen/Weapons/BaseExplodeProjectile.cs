using UnityEngine;

namespace Oxygen
{
	public abstract class BaseExplodeProjectile : BaseProjectile
	{
		[SerializeField] private float _radius;

		[SerializeField] private Explosion _defaultExplode;

		public void Explode()
		{
			Explosion explode = Instantiate(_defaultExplode, transform.position, Quaternion.identity);
			explode.Constuct(_radius, GetDamage());

			Destroy(gameObject);
		}
	}
}