using System;

namespace Oxygen
{
    public interface IReadableWeaponManager
    {
        event Action<BaseWeapon> WeaponChanged;
        
        BaseWeapon CurrentWeapon { get; }
        int CurrentWeaponIndex { get; }
    }
}