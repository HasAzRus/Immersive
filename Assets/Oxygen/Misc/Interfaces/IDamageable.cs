using UnityEngine;

namespace Oxygen
{
	public interface IDamageable
	{
		void ApplyDamage(GameObject caller, float damage);
	}
}