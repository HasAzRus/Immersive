using System;
using UnityEngine;

namespace Oxygen
{
	public class Localization
	{
		public static event Action<Language> LanguageChanged;

		private static Language _currentLanguage;

		private static LocalizationDatabase _database;
		
		public static void Initialize(LocalizationDatabase database)
		{
			_database = database;
		}
		
		private static bool TryGetItem(string name, out LocalizationItem item)
		{
			return _database.TryGetItem(name, out item);
		}

		private static bool TryGetTextByIndex(string name, int index, Language language, out string text)
		{
			text = string.Empty;

			if (!TryGetItem(name, out var item))
			{
				return false;
			}

			return item.TryGetValue(index, language, out text);
		}

		private static bool TryGetTextByIndex(string name, string category, int index, Language language, out string text)
		{
			text = string.Empty;

			if (!_database.TryGetValue(category, out var categoryItem))
			{
				return false;
			}

			if (!categoryItem.TryGetItem(name, out var item))
			{
				return false;
			}

			return item.TryGetValue(index, language, out text);
		}

		public static bool TryGetText(string name, out string text)
		{
			return TryGetText(name, _currentLanguage, out text);
		}

		public static bool TryGetText(string name, Language language, out string text)
		{
			return TryGetTextByIndex(name, 0, language, out text);
		}

		public static bool TryGetText(string name, string category, out string text) 
		{
			return TryGetText(name, category, _currentLanguage, out text);
		}

		public static bool TryGetText(string name, string category, Language language, out string text)
		{
			return TryGetTextByIndex(name, category, 0, language, out text);
		}

		public static bool TryGetRandomText(string name, Language language, out string text)
		{
			text = string.Empty;

			if (!TryGetItem(name, out LocalizationItem item))
			{
				return false;
			}

			return item.TryGetRandomValue(language, out text);
		}

		public static bool TryGetRandomText(string name, out string text)
		{
			return TryGetRandomText(name, _currentLanguage, out text);
		}

		public static bool TryGetRandomText(string name, string category, Language language, out string text)
		{
			text = string.Empty;

			if (!_database.TryGetValue(category, out var categoryItem))
			{
				return false;
			}

			return categoryItem.TryGetItem(name, out var item) && item.TryGetRandomValue(language, out text);
		}

		public static bool TryGetRandomText(string name, string category, out string text)
		{
			return TryGetRandomText(name, category, _currentLanguage, out text);
		}

		public static bool ChangeLanguage(Language language)
		{
			Debug.Log($"Язык был изменен на {language}");

			_currentLanguage = language;
			LanguageChanged?.Invoke(language);

			return true;
		}
	}
}
