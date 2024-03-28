using Oxygen;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oxygen.Editor
{
	[CustomEditor(typeof(Level))]
	public class LevelInspector : UnityEditor.Editor
	{
		private Level _target;

		private void OnEnable()
		{
			_target = (Level)target;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if(GUILayout.Button("Open Scene"))
			{
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					var path = _target.GetPath();
					
					try
					{
						EditorSceneManager.OpenScene(path);
					}
					catch
					{
						Debug.LogError($"Cannot load scene {path}");
					}
				}
			}

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Assign Current Scene"))
			{
				var scene = SceneManager.GetActiveScene();

				_target.SetName(scene.name);
				_target.SetPath(scene.path);
				
				EditorUtility.SetDirty(_target);
				AssetDatabase.SaveAssetIfDirty(_target);
			}

			if(GUILayout.Button("Set Startup Level"))
			{
				var launcher = AssetDatabase.LoadAssetAtPath<Launcher>(Preferences.DefaultLauncherPath);

				launcher.SetLevel(_target);
				
				EditorUtility.SetDirty(launcher);
				AssetDatabase.SaveAssetIfDirty(launcher);

				EditorGUIUtility.PingObject(launcher);
				Selection.activeObject = launcher;
			}

			if (GUILayout.Button("Check/Add to Build"))
			{
				LevelCreator.AddToBuild(_target);
			}

			GUILayout.EndHorizontal();
		}
	}
}