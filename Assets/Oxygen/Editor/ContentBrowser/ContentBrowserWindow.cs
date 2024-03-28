using UnityEditor;
using UnityEngine;

namespace Oxygen.Editor
{
	public class ContentBrowserWindow : EditorWindow
	{
		private ContentHubDatabase _hub;
		private ContentDatabase[] _databases;
		
		private ContentDatabase _database;

		private int _lastIndex;
		private int _index;

		private string[] _contentDatabaseNames;

		private Content[] _contents;
		private string[] _categories;

		private Texture2D _previewTexture;

		private string[] _skins;

		private int _selectedSkin;
		private int _lastSelectedSkin;

		private int _selectedContentIndex;
		private int _lastSelectedContentIndex;
		
		private bool _descriptionFoldout;
		private bool _clonerFoldout;

		private Vector2 _clonerScrollPosition;
		private bool _isClonerOverride;

		private bool _isDescEditing;
		private string _newDescription;

		private string _clonerName;

		private Vector3 _clonerPosition;
		private Vector3 _clonerRotation;
		private Vector3 _clonerScale;

		private void FinishDescriptionEditing()
		{
			_newDescription = string.Empty;
			_isDescEditing = false;
		}

		private void SelectContentDatabase(ContentDatabase contentDatabase)
		{
			_contents = contentDatabase.GetContents();
			_categories = new string[_contents.Length];

			for (var i = 0; i < _categories.Length; i++)
			{
				var content = _contents[i];

				_categories[i] = $"{content.GetCategory()}/{content.GetName()}";
			}

			_lastSelectedContentIndex = -1;
			_lastSelectedContentIndex = -1;
			
			_selectedContentIndex = 0;
			_selectedSkin = 0;

			_clonerScale = Vector3.one;

			_clonerName = _contents[_selectedContentIndex]?.GetName();
		}

		private void OnGUI()
		{
			var selectedContentDatabase = _databases[_index];

			if (_lastIndex != _index)
			{
				_lastIndex = _index;
				
				SelectContentDatabase(selectedContentDatabase);
			}
			
			_index = GUILayout.Toolbar(_index, _contentDatabaseNames);

			EditorGUILayout.Space(20);

			var selectedContent = _contents[_selectedContentIndex];

			var allowSkins = selectedContent.CheckAllowSkin();
			GameObject skin = null;

			if (allowSkins)
			{
				skin = selectedContent.GetSkins()[_selectedSkin];
				
				if(_lastSelectedSkin != _selectedSkin)
				{
					_lastSelectedSkin = _selectedSkin;

					_previewTexture = AssetPreview.GetAssetPreview(skin);

					_clonerName = $"{selectedContent.GetName()}_skin{_selectedSkin}";

					var clonerTransform = selectedContent.GetSkins()[_selectedSkin].transform;

					_clonerPosition = clonerTransform.position;
					_clonerRotation = clonerTransform.eulerAngles;
					_clonerScale = clonerTransform.localScale;
				}
			}

			if (_lastSelectedContentIndex != _selectedContentIndex)
			{
				_lastSelectedContentIndex = _selectedContentIndex;

				var clonerTransform = allowSkins ? selectedContent.GetSkins()[_selectedSkin].transform : 
					selectedContent.GetOriginal().transform;

				if (allowSkins)
				{
					_skins = new string[selectedContent.GetSkins().Length];
					_selectedSkin = 0;

					for (var i = 0; i < _skins.Length; i++)
					{
						_skins[i] = $"Skin {i + 1}";
					}

					_previewTexture = AssetPreview.GetAssetPreview(selectedContent.GetSkins()[_selectedSkin]);

					_clonerName = $"{selectedContent.GetName()}_skin{_selectedSkin}";
				}
				else
				{
					_previewTexture = AssetPreview.GetAssetPreview(selectedContent.GetOriginal());

					_clonerName = selectedContent.GetName();
				}

				_clonerPosition = clonerTransform.position;
				_clonerRotation = clonerTransform.eulerAngles;
				_clonerScale = clonerTransform.localScale;
			}

			EditorGUILayout.BeginHorizontal();

			if (selectedContent.GetIcon() != null)
			{
				GUI.DrawTexture(new Rect(5, 5, 30, 30), selectedContent.GetIcon());
			}
			else
			{
				var defaultTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Preferences.ClonerDefaultIconPath);

				GUI.DrawTexture(new Rect(5, 35, 50, 50), _previewTexture == null ? defaultTexture : _previewTexture);
			}

			EditorGUILayout.Space(60);

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();

			GUILayout.Label("Select content:");
			_selectedContentIndex = EditorGUILayout.Popup(_selectedContentIndex, _categories);

			GUILayout.EndHorizontal();

			if (allowSkins)
			{
				_selectedSkin = EditorGUILayout.Popup("Select skin:", _selectedSkin, _skins);
			}

			GUILayout.EndVertical();

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("<"))
			{
				if(_selectedContentIndex - 1 > 0)
				{
					_selectedContentIndex -= 1;
				}
				else
				{
					_selectedContentIndex = 0;
				}
			}

			if(GUILayout.Button(">"))
			{
				if (_selectedContentIndex + 1 < _contents.Length - 1)
				{
					_selectedContentIndex += 1;
				}
				else
				{
					_selectedContentIndex = _contents.Length - 1;
				}
			}

			GUILayout.EndHorizontal();

			if (allowSkins)
			{
				GUILayout.BeginHorizontal();

				if (GUILayout.Button("<"))
				{
					if (_selectedSkin - 1 > 0)
					{
						_selectedSkin -= 1;
					}
					else
					{
						_selectedSkin = 0;
					}
				}

				if (GUILayout.Button(">"))
				{
					if (_selectedSkin + 1 < _skins.Length - 1)
					{
						_selectedSkin += 1;
					}
					else
					{
						_selectedSkin = _skins.Length - 1;
					}
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();

			if(GUILayout.Button("Select from assets"))
			{
				var obj = allowSkins ? skin : selectedContent.GetOriginal();

				EditorGUIUtility.PingObject(obj);
				Selection.activeObject = obj;
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(15);

			_descriptionFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_descriptionFoldout, "Description");

			if (_descriptionFoldout)
			{
				if (!_isDescEditing)
				{
					var description = selectedContent.GetDescription();
					GUILayout.Label(description != string.Empty ? description : "...");

					if (GUILayout.Button("Edit"))
					{
						_newDescription = selectedContent.GetDescription();

						_isDescEditing = true;
					}
				}
				else
				{
					_newDescription = GUILayout.TextArea(_newDescription);

					GUILayout.BeginHorizontal();

					if (GUILayout.Button("Save"))
					{
						selectedContent.SetDescription(_newDescription);

						FinishDescriptionEditing();
					}

					if (GUILayout.Button("Cancel"))
					{
						FinishDescriptionEditing();
					}

					GUILayout.EndHorizontal();
				}
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			_clonerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_clonerFoldout, "Cloner");

			if (_clonerFoldout)
			{
				_clonerScrollPosition = EditorGUILayout.BeginScrollView(_clonerScrollPosition);

				_isClonerOverride = EditorGUILayout.BeginToggleGroup("Override", _isClonerOverride);

				_clonerName = EditorGUILayout.TextField("Name:", _clonerName);

				_clonerPosition = EditorGUILayout.Vector3Field("Position: ", _clonerPosition);
				_clonerRotation = EditorGUILayout.Vector3Field("Rotation:", _clonerRotation);
				_clonerScale = EditorGUILayout.Vector3Field("Scale", _clonerScale);

				if (GUILayout.Button("Apply selected transform"))
				{
					var selectedTransform = Selection.activeGameObject.transform;

					_clonerPosition = selectedTransform.position;
					_clonerRotation = selectedTransform.eulerAngles;
					_clonerScale = selectedTransform.localScale;
				}

				EditorGUILayout.EndToggleGroup();

				EditorGUILayout.EndScrollView();

				GUILayout.Space(10);
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			if(GUILayout.Button("Create a clone"))
			{
				var sceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;

				var clonerPosition = !_isClonerOverride ? sceneViewCameraTransform.position + sceneViewCameraTransform.forward * Preferences.ClonerDistance : _clonerPosition;

				if (Preferences.ClonerToggleTo2D)
				{
					clonerPosition.z = 0f;
				}
				
				var clone = PrefabUtility.InstantiatePrefab(allowSkins ? skin : selectedContent.GetOriginal()) as GameObject;
				var cloneTransform = clone.transform;

				cloneTransform.position = clonerPosition;

				if (_clonerName != "")
				{
					clone.name = _clonerName;
				}

				Undo.RegisterCreatedObjectUndo(clone, _clonerName);

				EditorGUIUtility.PingObject(clone);
				Selection.activeGameObject = clone;

				if (_isClonerOverride)
				{
					SceneView.lastActiveSceneView.LookAt(clone.transform.position);
				}
			}

			GUILayout.BeginHorizontal();

			if(GUILayout.Button("Select database from assets"))
			{
				EditorGUIUtility.PingObject(selectedContentDatabase);
				Selection.activeObject = selectedContentDatabase;
			}

			if(GUILayout.Button("Reset"))
			{
				ResetContent();
			}

			GUILayout.EndHorizontal();
		}

		[ContextMenu("Reset Content")]
		private void ResetContent()
		{
			var hub = AssetDatabase.LoadAssetAtPath<ContentHubDatabase>(Preferences.ClonerDatabasePath);

			if (hub == null)
			{
				return;
			}

			_hub = hub;
			_index = 0;

			_lastIndex = -1;
			_lastSelectedSkin = -1;
			_lastSelectedContentIndex = -1;

			_databases = _hub.GetContentDatabase();
			_contentDatabaseNames = new string[_databases.Length];

			for (var i = 0; i < _contentDatabaseNames.Length; i++)
			{
				_contentDatabaseNames[i] = _databases[i].GetName();
			}
			
			SelectContentDatabase(_databases[_index]);
		}

		[MenuItem("Oxygen/Content Browser")]
		private static void CreateWindow()
		{
			var window = GetWindow<ContentBrowserWindow>("Content Browser");
			window.ResetContent();
		}
	}
}