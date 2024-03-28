using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Oxygen.Editor
{
	[InitializeOnLoad]
	public static class AutoSceneLoader
	{
		static AutoSceneLoader()
		{
			EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
		}

		private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange obj)
		{
			if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			{
				if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
				{
					return;
				}

				var path = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
				EditorPrefs.SetString(Preferences.ActiveScenePrefKey, path);

				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					try
					{
						EditorSceneManager.OpenScene(Preferences.DefaultEntryScenePath);
					}
					catch
					{
						Debug.LogError($"Cannot load scene {Preferences.DefaultEntryScenePath}");
						EditorApplication.isPlaying = false;
					}
				}
				else
				{
					EditorApplication.isPlaying = false;
				}
			}


			if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				var path = EditorPrefs.GetString(Preferences.ActiveScenePrefKey);

				try
				{
					EditorSceneManager.OpenScene(path);
				}
				catch
				{
					Debug.LogError($"Cannot load scene: {path}");
				}
			}
		}
	}
}