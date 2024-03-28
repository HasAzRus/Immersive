using UnityEditor;
using UnityEngine;

namespace Oxygen.Editor
{
	public class PreferencesWindow : EditorWindow
	{
		private bool _clonerFoldout;

		private float _clonerDistance;
		private string _clonerDatabasePath;
		private string _clonerDefaultIconPath;

		private bool _clonerToggleTo2d;

		private Texture2D _clonerDefaultIcon;

		private bool _gizmosFoldout;
		
		private Color _touchTriggerColor;
		private Color _killZoneColor;
		private Vector3 _playerStartSize;

		private bool _launcherFoldout;
		private string _defaultLauncherPath;
		private string _defaultLevelsPath;

		private bool _autoSceneLoaderFoldout;
		private string _defaultEntryScenePath;
		private string _activeScenePrefKey;

		private void Load()
		{
			PreferencesEditor.Load();

			_clonerDistance = Preferences.ClonerDistance;
			_clonerDatabasePath = Preferences.ClonerDatabasePath;

			_clonerDefaultIcon = AssetDatabase.LoadAssetAtPath(Preferences.ClonerDefaultIconPath, typeof(Texture2D)) as Texture2D;

			_clonerToggleTo2d = Preferences.ClonerToggleTo2D;
			
			_touchTriggerColor = Preferences.TouchTriggerColor;
			_killZoneColor = Preferences.KillZoneColor;

			_playerStartSize = Preferences.PlayerStartSize;

			_defaultLauncherPath = Preferences.DefaultLauncherPath;
			_defaultLevelsPath = Preferences.DefaultLevelsPath;
			
			_defaultEntryScenePath = Preferences.DefaultEntryScenePath;
			_activeScenePrefKey = Preferences.ActiveScenePrefKey;
		}

		private void Save()
		{
			Preferences.ClonerDistance = _clonerDistance;
			Preferences.ClonerDatabasePath = _clonerDatabasePath;
			Preferences.ClonerDefaultIconPath = AssetDatabase.GetAssetPath(_clonerDefaultIcon);

			Preferences.TouchTriggerColor = _touchTriggerColor;
			Preferences.KillZoneColor = _killZoneColor;

			Preferences.PlayerStartSize = _playerStartSize;

			Preferences.ClonerToggleTo2D = _clonerToggleTo2d;

			Preferences.DefaultLauncherPath = _defaultLauncherPath;
			Preferences.DefaultLevelsPath = _defaultLevelsPath;
			
			Preferences.DefaultEntryScenePath = _defaultEntryScenePath;
			Preferences.ActiveScenePrefKey = _activeScenePrefKey;

			PreferencesEditor.Save();
		}

		private void OnGUI()
		{
			_clonerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_clonerFoldout, "Cloner");

			if (_clonerFoldout)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label("Distance");
				_clonerDistance = EditorGUILayout.FloatField(_clonerDistance);

				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();

				GUILayout.Label("Database Path");
				_clonerDatabasePath = EditorGUILayout.TextField(_clonerDatabasePath);
				
				if (GUILayout.Button("Select from assets"))
				{
					OxEditor.CheckAssetByPath<ContentHubDatabase>(_clonerDatabasePath);
				}

				GUILayout.EndHorizontal();

				_clonerDefaultIcon = EditorGUILayout.ObjectField("Default Icon", _clonerDefaultIcon, typeof(Texture2D), false) as Texture2D;

				_clonerToggleTo2d = EditorGUILayout.Toggle("Toggle to 2D", _clonerToggleTo2d);
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			_gizmosFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_gizmosFoldout, "Gizmos");

			if(_gizmosFoldout)
			{
				_touchTriggerColor = EditorGUILayout.ColorField("Trigger Color", _touchTriggerColor);
				_killZoneColor = EditorGUILayout.ColorField("Killzone Color", _killZoneColor);
				_playerStartSize = EditorGUILayout.Vector3Field("Player Start Size", _playerStartSize);
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			_launcherFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_launcherFoldout, "Launcher");

			if (_launcherFoldout)
			{
				EditorGUILayout.BeginHorizontal();
				
				GUILayout.Label("Default Launcher Path");
				_defaultLauncherPath = EditorGUILayout.TextField(_defaultLauncherPath);

				if (GUILayout.Button("Select from assets"))
				{
					OxEditor.CheckAssetByPath<Launcher>(_defaultLauncherPath);
				}

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				
				GUILayout.Label("Default Levels Path");
				_defaultLevelsPath = EditorGUILayout.TextField(_defaultLevelsPath);
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.EndFoldoutHeaderGroup();

			_autoSceneLoaderFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_autoSceneLoaderFoldout,"Auto-scene Loader");

			if (_autoSceneLoaderFoldout)
			{
				EditorGUILayout.BeginHorizontal();

				GUILayout.Label("Default Entry Scene Path");
				_defaultEntryScenePath = EditorGUILayout.TextField(_defaultEntryScenePath);

				if (GUILayout.Button("Select from assets"))
				{
					OxEditor.CheckAssetByPath<SceneAsset>(_defaultEntryScenePath);
				}

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				
				GUILayout.Label("Active Scene Pref Key");
				_activeScenePrefKey = EditorGUILayout.TextField(_activeScenePrefKey);
				
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndFoldoutHeaderGroup();
			
			if (GUILayout.Button("Save"))
			{
				Save();
			}
		}

		private void OnEnable()
		{
			Load();
		}

		private void OnDisable()
		{
			Save();
		}

		[MenuItem("Oxygen/Preferences...")]
		private static void CreateWindow()
		{
			GetWindow<PreferencesWindow>("Preferences");
		}
	}
}