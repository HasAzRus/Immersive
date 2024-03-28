using UnityEngine;

namespace Oxygen.Editor
{
	[CreateAssetMenu(fileName = "New Content Database", menuName = "Oxygen/Editor/Content Database")]
	public class ContentDatabase : ScriptableObject
	{
		[SerializeField] private string _name;
		[SerializeField] private Content[] _contents;

		public string GetName()
		{
			return _name;
		}
		
		public Content[] GetContents()
		{
			return _contents;
		}
	}
}