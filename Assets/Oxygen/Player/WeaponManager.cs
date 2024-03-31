using System;
using UnityEngine;

namespace Oxygen
{
	public class WeaponManager : Behaviour, IReadableWeaponManager
	{
		public event Action<BaseWeapon> WeaponChanged;

		[SerializeField] private BaseWeapon[] _weapons;

		public void Construct(Character owner)
		{
			CurrentWeaponIndex = -1;

			foreach (var weapon in _weapons)
			{
				weapon.Construct(owner);
			}
		}

		protected virtual void OnConstruction(Character owner)
		{
			
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
			
			WeaponChanged?.Invoke(null);

			return true;
		}

		public void SetWeaponActive(int index, bool value)
		{
			_weapons[index].SetActive(value);
		}

		public bool CheckWeaponActive(int index)
		{
			return _weapons[index].IsActive;
		}
		
		public BaseWeapon CurrentWeapon { get; private set; }
		public int CurrentWeaponIndex { get; private set; }
	}
}