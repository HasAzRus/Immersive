using System;
using UnityEngine;

namespace Oxygen
{
	[CreateAssetMenu(fileName = "New Level", menuName = "Oxygen/Levels/Level")]
	public class Level : ScriptableObject, ILevel
	{
		[SerializeField] private string _name;
		[SerializeField] private string _path;

		public void SetName(string value)
		{
			_name = value;
		}

		public void SetPath(string value)
		{
			_path = value;
		}

		public string Name => _name;
		public string Path => _path;
	}
}
