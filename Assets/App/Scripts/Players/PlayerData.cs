using System;
using App.Weapons;
using UnityEngine;

namespace App.Players
{
    public static class PlayerData
    {
        public static string NickName = "Anonymous";
        public static WeaponId SelectedWeapon { get; private set; }  = WeaponId.Scar;
        public static int EquippedArmorLevel { get; private set; }  = 1;

        public static event Action OnWeaponChanged;
        public static event Action OnArmorLevelChanged;
        
        public static void  SetWeapon(WeaponId weaponId)
        {
            SelectedWeapon = weaponId;
            OnWeaponChanged?.Invoke();
        }
        
        public static void  EquipArmor(int armorLevel)
        {
            if (EquippedArmorLevel == armorLevel)
            {
                Debug.LogWarning("You try equip armor that already equipped");
                return;
            }
            
            EquippedArmorLevel = armorLevel;
            OnArmorLevelChanged?.Invoke();
        }
    }
}