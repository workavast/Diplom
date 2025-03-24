using System;
using App.Weapons;

namespace App.Players
{
    public static class PlayerData
    {
        public static string NickName = "Anonymous";
        public static WeaponId SelectedWeapon { get; private set; }  = WeaponId.Scar;

        public static event Action OnWeaponChanged;
        
        public static void  SetWeapon(WeaponId weaponId)
        {
            SelectedWeapon = weaponId;
            OnWeaponChanged?.Invoke();
        }
    }
}