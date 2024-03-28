using System;
using UnityEngine;

namespace Oxygen
{
	public class WeaponManager : Behaviour
	{
		public event Action<int> WeaponChanged;

		[SerializeField] private BaseWeapon[] _weapons;

		private BaseWeapon _currentWeapon;
		private int _currentWeaponIndex;

		private GameObject _owner;
		
		public void Construct(GameObject owner)
		{
			_owner = owner;
		}

		public void Initialize()
		{
			_currentWeaponIndex = -1;

			foreach (var weapon in _weapons)
			{
				weapon.Construct(_owner);
			}
		}

		public bool ChangeWeapon(int index)
		{
			if(_currentWeapon != null)
			{
				_currentWeapon.Deselect();
			}

			if(index < 0)
			{
				_currentWeapon = null;
				_currentWeaponIndex = -1;

				WeaponChanged?.Invoke(-1);
				
				return true;
			}

			var weapon = _weapons[index];

			if(!weapon.CheckActive())
			{
				return false;
			}

			weapon.Select();

			_currentWeapon = weapon;
			_currentWeaponIndex = index;

			WeaponChanged?.Invoke(index);

			return true;
		}

		public void SetWeaponActive(int index, bool value)
		{
			_weapons[index].SetActive(value);
		}

		public BaseWeapon GetWeapon(int index) 
		{
			return _weapons[index];
		}

		public BaseWeapon[] GetWeapons()
		{
			return _weapons;
		}

		public BaseWeapon GetCurrentWeapon()
		{
			return _currentWeapon;
		}

		public int GetCurrentWeaponIndex()
		{
			return _currentWeaponIndex;
		}
	}
}