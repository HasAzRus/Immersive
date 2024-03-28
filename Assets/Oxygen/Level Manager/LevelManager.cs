using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oxygen
{
	public enum LoadLevelMode
	{
		Single,
		Additive
	}

	public class LevelManager
	{
		public static event Action Loading;
		public static event Action<ILevel, LoadLevelMode> Loaded;

		public static event Action<ILevel> Unloaded;

		private static Level _currentLevel;

		private static IEnumerator LoadRoutine(Level level, LoadLevelMode loadlLevelMode)
		{
			if (loadlLevelMode == LoadLevelMode.Single)
			{
				if (_currentLevel != null)
				{
					yield return UnloadRoutine(_currentLevel);
				}
			}
			
			Loading?.Invoke();

			var loadOperation = SceneManager.LoadSceneAsync(level.Name, LoadSceneMode.Additive);
			loadOperation.allowSceneActivation = false;

			while(!loadOperation.isDone) 
			{
				if(loadOperation.progress >= 0.9f)
				{
					loadOperation.allowSceneActivation = true;
				}

				yield return null;
			}

			_currentLevel = level;
			
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(level.Name));

			Loaded?.Invoke(level, loadlLevelMode);
		}

		private static IEnumerator UnloadRoutine(ILevel level)
		{
			yield return SceneManager.UnloadSceneAsync(level.Name);

			Unloaded?.Invoke(level);
		}

		public static void Load(Level level, LoadLevelMode loadLevelMode)
		{
			Coroutines.Run(LoadRoutine(level, loadLevelMode));
		}
		
		public static void Unload(Level level) 
		{
			Coroutines.Run(UnloadRoutine(level));
		}
	}
}