using UnityEngine;

namespace Oxygen
{
	public class Bootstrap : Behaviour
	{
		[SerializeField] private Game _defaultGame;

		[SerializeField] private GameObject[] _autoSpawnGameObjects;

		[SerializeField] private LocalizationDatabase _localizationDatabase;
		[SerializeField] private Language _initialLanguage;

		[SerializeField] private string _saveProfileName;
		[SerializeField] private int _saveProfilesCount;

		protected override void Start()
		{
			base.Start();

			Debug.Log("Старт системы");
			
			SaveLoad.Initialize(_saveProfileName, _saveProfilesCount);
			
			Localization.Initialize(_localizationDatabase);
			Localization.ChangeLanguage(_initialLanguage);
			
			Coroutines.Initialize();
			
			TimeInvoker.Initialize();

			foreach (var autoSpawnGameObject in _autoSpawnGameObjects)
			{
				Instantiate(autoSpawnGameObject);
			}
			
			Instantiate(_defaultGame);
		}
	}
}