using System;
using UnityEngine;

namespace Oxygen
{
	[Serializable]
	public class Translation
	{
		[SerializeField] private Language _language;
		[SerializeField][Multiline] private string[] _values;

		public Language GetLanguage()
		{
			return _language;
		}

		public string GetValue(int index)
		{
			return _values[index];
		}

		public int GetLength()
		{
			return _values.Length;
		}
	}

	[Serializable]
	public class LocalizationItem
	{
		[SerializeField] private string _name;

		[SerializeField] private Translation[] _translations;

		private bool TryGetTranslation(Language language, out Translation translation)
		{
			translation = null;

			foreach (var other in _translations)
			{
				if (other.GetLanguage() == language)
				{
					translation = other;
					return true;
				}
			}

			return false;
		}

		public string GetName()
		{
			return _name;
		}

		public bool TryGetValue(int index, Language language, out string value)
		{
			value = string.Empty;

			if (!TryGetTranslation(language, out var translation))
			{
				return false;
			}

			value = translation.GetValue(index);

			return true;
		}

		public bool TryGetRandomValue(Language language, out string value)
		{
			value = string.Empty;

			if (!TryGetTranslation(language, out var translation))
			{
				return false;
			}

			value = translation.GetValue(UnityEngine.Random.Range(0, translation.GetLength()));

			return true;
		}
	}

	[CreateAssetMenu(fileName = "New Localization Category", menuName = "Oxygen/Databases/Localization/Category")]
	public class LocalizationCategory : ScriptableObject
	{
		[SerializeField] private string _name;

		[SerializeField] private LocalizationItem[] _items;
		
		public string GetName()
		{
			return _name;
		}

		public bool TryGetItem(string name, out LocalizationItem item)
		{
			item = null;

			foreach (var other in _items)
			{
				if (other.GetName() != name)
				{
					continue;
				}

				item = other;

				return true;
			}

			return false;
		}

		public LocalizationItem[] GetItems()
		{
			return _items;
		}
	}
}