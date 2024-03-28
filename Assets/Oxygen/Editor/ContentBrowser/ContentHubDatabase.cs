using System;
using UnityEngine;

namespace Oxygen.Editor
{
    [CreateAssetMenu(fileName = "New Content Hub Database", menuName = "Oxygen/Editor/Content Hub Database")]
    public class ContentHubDatabase : ScriptableObject
    {
        [SerializeField] private ContentDatabase[] _contentDatabases;

        public ContentDatabase[] GetContentDatabase()
        {
            return _contentDatabases;
        }
    }
}