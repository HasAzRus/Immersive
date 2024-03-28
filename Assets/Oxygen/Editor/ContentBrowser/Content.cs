using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oxygen.Editor
{
	[Serializable]
	public class Content
	{
		[SerializeField] private string _name;
		[SerializeField] [Multiline] private string _description;

		[SerializeField] private string _category;

		[SerializeField] private Texture2D _icon;

		[SerializeField] private GameObject _original;

		[SerializeField] private bool _allowSkin;
		[SerializeField] private GameObject[] _skins;

		public string GetName()
		{
			return _name;
		}

		public void SetDescription(string value)
		{
			_description = value;
		}

		public string GetDescription()
		{
			return _description;
		}

		public string GetCategory()
		{
			return _category;
		}

		public Texture2D GetIcon()
		{
			return _icon;
		}

		public GameObject GetOriginal()
		{
			return _original;
		}

		public bool CheckAllowSkin()
		{
			return _allowSkin;
		}

		public GameObject[] GetSkins()
		{
			return _skins;
		}
	}
}