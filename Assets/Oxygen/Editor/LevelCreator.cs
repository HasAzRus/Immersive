using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Oxygen.Editor
{
	public class LevelCreator
	{
		[MenuItem("Oxygen/Levels/Create level from current scene")]
		private static void CreateLevelFromCurrentScene()
		{
			Create(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
		}

		public static void Create(UnityEngine.SceneManagement.Scene scene)
		{
			var path = Preferences.DefaultLevelsPath;
			
			var level = ScriptableObject.CreateInstance<Level>();
			
			level.SetName(scene.name);
			level.SetPath(scene.path);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			AssetDatabase.CreateAsset(level, $"{path}{scene.name}_Level.asset");
			AssetDatabase.SaveAssets();

			AddToBuild(level);
		}

		public static void AddToBuild(Level level)
		{
			var buildScenes = EditorBuildSettings.scenes;

			if (buildScenes.Any(buildScene => buildScene.path == level.GetPath()))
			{
				Debug.LogError($"Данная сцена ({level.Name}) уже занесена в список Build");
				
				return;
			}

			var editorBuildSettingsScene = new EditorBuildSettingsScene(level.GetPath(), true);
			var scenes = new EditorBuildSettingsScene[buildScenes.Length + 1];

			for (var i = 0; i < buildScenes.Length; i++)
			{
				scenes[i] = buildScenes[i];
			}

			scenes[buildScenes.Length] = editorBuildSettingsScene;

			EditorBuildSettings.scenes = scenes;
			
			Debug.Log($"<color=green>Сцена ({level.Name}) была успешно занесена в список Build</color>");
		}
	}
}