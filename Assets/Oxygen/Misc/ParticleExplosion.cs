using UnityEngine;

namespace Oxygen
{
	public class ParticleExplosion : Explosion
	{
		[SerializeField] private ParticleSystem _particle;

		protected override void Start()
		{
			base.Start();

			_particle.transform.parent = null;
		}
	}
}