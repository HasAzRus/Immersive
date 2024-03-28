using UnityEngine;

namespace Oxygen
{
	public class BaseParticleTraceFirearmWeapon : BaseTraceFirearmWeapon
	{
		[Header("Particle")]

		[SerializeField] private ParticleSystem _defaultHitParticleSystem;

		[SerializeField] private bool _usePooledParticleSystem;
		
		[SerializeField] private string _poolerName;
		[SerializeField] private string _poolerParticleName;

		protected override void OnTraced(RaycastHit hit)
		{
			base.OnTraced(hit);

			if (_usePooledParticleSystem)
			{
				if (!Pooler.TryGetPooler(_poolerName, out var pooler))
				{
					return;
				}
				
				var particleTransform = pooler.GetGameObject(_poolerParticleName).transform;

				particleTransform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));
			}
			else
			{
				Instantiate(_defaultHitParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
			}
		}
	}
}