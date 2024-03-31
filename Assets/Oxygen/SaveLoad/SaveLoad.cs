using System;

namespace Oxygen
{
	public class SaveLoad
	{
		public static event Action<IWritableStorage> Saving;
		public static event Action<IReadableStorage> Loading; 

		private static GameStorage[] _storages;
		private static GameStorage _currentStorage;

		public static void Initialize(string name, int count, int index = 0)
		{
			_storages = new GameStorage[count];
			
			for (var i = 0; i < count; i++)
			{
				_storages[i] = new GameStorage($"{name}_{i}");
			}
			
			SetProfile(index);
		}

		public static void Load()
		{
			_currentStorage.Load();
			Loading?.Invoke(_currentStorage);
		}

		public static void Save(bool force)
		{
			Saving?.Invoke(_currentStorage);

			if (force)
			{
				_currentStorage.Save();
			}
		}

		public static void SetProfile(int index)
		{
			_currentStorage = _storages[index];
		}
	}
}
