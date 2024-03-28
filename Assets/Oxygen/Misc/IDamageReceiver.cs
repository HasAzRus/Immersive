using UnityEngine;

namespace Oxygen
{
	public interface IDamageReceiver
	{
		void ApplyDamage(GameObject caller, float damage);
	}
}