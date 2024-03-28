namespace Oxygen
{
	public class SaveLoad
	{
		private static GameStorage[] _storages;
		
		public static void Initialize(string name, int count)
		{
			_storages = new GameStorage[count];
			
			for (var i = 0; i < count; i++)
			{
				_storages[i] = new GameStorage($"{name}_{i}");
			}
		}

		public static void Load(int index)
		{
			_storages[index].Load();
		}

		public static void Save(int index)
		{
			_storages[index].Save();
		}
	}
}
