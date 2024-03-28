using UnityEngine;

namespace Oxygen
{
	[CreateAssetMenu(fileName = "New Launcher", menuName = "Oxygen/Levels/Launcher")]
	public class Launcher : ScriptableObject
	{
		[SerializeField] private Level _level;

		public void SetLevel(Level level)
		{
			_level = level;
		}

		public Level GetLevel()
		{
			return _level;
		}
	}
}