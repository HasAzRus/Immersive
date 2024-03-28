using System;
using UnityEngine;

namespace Oxygen
{
	[CreateAssetMenu(fileName = "New Localization Database", menuName = "Oxygen/Databases/Localization")]
	public class LocalizationDatabase : ScriptableObject
	{
		[SerializeField] private LocalizationCategory[] _categories; 

		public bool TryGetItem(string name, out LocalizationItem item)
		{
			item = null;

			foreach (var category in _categories)
			{
				foreach (var other in category.GetItems())
				{
					if (other.GetName() != name)
					{
						continue;
					}

					item = other;

					return true;
				}
			}

			return false;
		}

		public bool TryGetCategory(string name, out LocalizationCategory category)
		{
			category = null;

			foreach (var other in _categories)
			{
				if (other.GetName() != name)
				{
					continue;
				}

				category = other;

				return true;
			}

			return false;
		}
	}
}
