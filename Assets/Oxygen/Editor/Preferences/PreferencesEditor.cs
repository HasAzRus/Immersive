using UnityEditor;
using UnityEngine;

namespace Oxygen.Editor
{
	[InitializeOnLoad]
	public static class PreferencesEditor
	{
		static PreferencesEditor()
		{
			Load();
		}

		public static void Load()
		{
			Preferences.ClonerDistance = EditorPrefs.GetFloat("clonerDistance", 4);
			Preferences.ClonerDatabasePath = EditorPrefs.GetString("clonerDatabasePath",
				"Assets/Content/Oxygen/Databases/Default Content Hub Database.asset");
			
			Preferences.ClonerDefaultIconPath = EditorPrefs.GetString("clonerDefaultIcon");
			Preferences.ClonerToggleTo2D = EditorPrefs.GetBool("clonerToggle2D", false);

			Preferences.TouchTriggerColor = new Color(EditorPrefs.GetFloat("touchTriggerColorR", 0f),
				                                      EditorPrefs.GetFloat("touchTriggerColorG", 1f),
													  EditorPrefs.GetFloat("touchTriggerColorB", 0f),
													  EditorPrefs.GetFloat("touchTriggerColorA", 0.2f));

			Preferences.KillZoneColor = new Color(EditorPrefs.GetFloat("killZoneColorR", 1f),
										          EditorPrefs.GetFloat("killZoneColorG", 0f),
										          EditorPrefs.GetFloat("killZoneColorB", 0f),
										          EditorPrefs.GetFloat("killZoneColorA", 0.2f));
			
			Preferences.PlayerStartSize = new Vector3(EditorPrefs.GetFloat("playerStartSizeX", 1f),
				                                      EditorPrefs.GetFloat("playerStartSizeY", 2f),
				                                      EditorPrefs.GetFloat("playerStartSizeZ", 1f));

			Preferences.DefaultLauncherPath =
				EditorPrefs.GetString("defaultLauncherPath", "Assets/Content/Launcher.asset");

			Preferences.DefaultLevelsPath = EditorPrefs.GetString("defaultLevelsPath", "Assets/Content/Levels/");

			Preferences.DefaultEntryScenePath = EditorPrefs.GetString("autoSceneLoaderDefaultEntryScenePath",
				"Assets/Content/Scenes/Entry Scene.unity");
			Preferences.ActiveScenePrefKey =
				EditorPrefs.GetString("autoSceneLoaderActiveScenePrefKey", "autoLoaderActiveScene");
		}

		public static void Save()
		{
			EditorPrefs.SetFloat("clonerDistance", Preferences.ClonerDistance);
			EditorPrefs.SetString("clonerDatabasePath", Preferences.ClonerDatabasePath);
			EditorPrefs.SetString("clonerDefaultIcon", Preferences.ClonerDefaultIconPath);
			EditorPrefs.SetBool("clonerToggle2D", Preferences.ClonerToggleTo2D);
			
			EditorPrefs.SetFloat("touchTriggerColorR", Preferences.TouchTriggerColor.r);
			EditorPrefs.SetFloat("touchTriggerColorG", Preferences.TouchTriggerColor.g);
			EditorPrefs.SetFloat("touchTriggerColorB", Preferences.TouchTriggerColor.b);
			EditorPrefs.SetFloat("touchTriggerColorA", Preferences.TouchTriggerColor.a);

			EditorPrefs.SetFloat("killZoneColorR", Preferences.KillZoneColor.r);
			EditorPrefs.SetFloat("killZoneColorG", Preferences.KillZoneColor.g);
			EditorPrefs.SetFloat("killZoneColorB", Preferences.KillZoneColor.b);
			EditorPrefs.SetFloat("killZoneColorA", Preferences.KillZoneColor.a);
			
			EditorPrefs.SetFloat("playerStartSizeX", Preferences.PlayerStartSize.x);
			EditorPrefs.SetFloat("playerStartSizeY", Preferences.PlayerStartSize.y);
			EditorPrefs.SetFloat("playerStartSizeZ", Preferences.PlayerStartSize.z);
			
			EditorPrefs.SetString("defaultLauncherPath", Preferences.DefaultLauncherPath);
			EditorPrefs.SetString("defaultLevelsPath", Preferences.DefaultLevelsPath);
			
			EditorPrefs.SetString("autoSceneLoaderDefaultEntryScenePath", Preferences.DefaultEntryScenePath);
			EditorPrefs.SetString("autoSceneLoaderActiveScenePrefKey", Preferences.ActiveScenePrefKey);
		}
	}
}