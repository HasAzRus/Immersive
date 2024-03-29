using UnityEngine;

namespace Oxygen
{
	public interface IKillable
	{
		bool Kill(GameObject caller);
	}
}