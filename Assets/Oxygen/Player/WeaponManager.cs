using System;
using UnityEngine;

namespace Oxygen
{
	public class WeaponManager : Behaviour
	{
		public event Action<BaseWeapon> WeaponChanged;

		[SerializeField] private BaseWeapon[] _weapons;

		private GameObject _owner;
		
		public void Construct(GameObject owner)
		{
			_owner = owner;
			
			CurrentWeaponIndex = -1;

			foreach (var weapon in _weapons)
			{
				weapon.Construct(_owner);
			}
		}

		public bool ChangeWeapon(int index)
		{
			var weapon = _weapons[index];

			if(!weapon.IsActive)
			{
				Debug.Log($"Оружие ({weapon.Name}) недоступно");
				
				return false;
			}
			
			if(CurrentWeapon != null)
			{
				if (CurrentWeapon == weapon)
				{
					Debug.Log($"Оружие ({weapon.Name}) уже выбрано");
					
					return false;
				}
				
				CurrentWeapon.Deselect();
			}
			
			weapon.Select();

			CurrentWeapon = weapon;
			CurrentWeaponIndex = index;

			WeaponChanged?.Invoke(CurrentWeapon);

			return true;
		}

		public bool DeselectCurrentWeapon()
		{
			if (CurrentWeapon == null)
			{
				return false;
			}
			
			CurrentWeapon.Deselect();

			CurrentWeapon = null;
			CurrentWeaponIndex = -1;

			return true;
		}

		public void SetWeaponActive(int index, bool value)
		{
			_weapons[index].SetActive(value);
		}
		
		public BaseWeapon CurrentWeapon { get; private set; }
		public int CurrentWeaponIndex { get; private set; }
	}
}