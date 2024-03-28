using UnityEditor;
using UnityEngine;

namespace Oxygen.Editor
{
    public class OxEditor
    {
        public static bool CheckAssetByPath<T>(string path) where T : Object
        {
            var obj = AssetDatabase.LoadAssetAtPath<T>(path);

            if (obj == null)
            {
                return false;
            }
            
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;

            return true;
        }
    }
}