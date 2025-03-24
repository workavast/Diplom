using System.Collections.Generic;
using App.Players;
using Zenject;

namespace App.Weapons
{
    public class WeaponSelector
    {
        [Inject] private readonly WeaponsConfigs _weaponsConfigs;
        
        public void SelectWeapon(WeaponId weaponId) 
            => PlayerData.SetWeapon(weaponId);

        public IReadOnlyList<WeaponId> GetAllWeapon() 
            => _weaponsConfigs.WeaponIds;
    }
}