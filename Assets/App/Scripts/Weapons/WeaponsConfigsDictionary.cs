using System.Collections.Generic;
using UnityEngine;

namespace App.Weapons
{
    public class WeaponsConfigsDictionary
    {
        private readonly Dictionary<WeaponId, WeaponConfig> _weaponConfigs;
        
        public IReadOnlyDictionary<WeaponId, WeaponConfig> WeaponConfigs => _weaponConfigs;
        
        public WeaponsConfigsDictionary(IReadOnlyCollection<WeaponConfig> weaponConfigs)
        {
            _weaponConfigs = new Dictionary<WeaponId, WeaponConfig>(weaponConfigs.Count);

            foreach (var weaponConfig in weaponConfigs)
            {
                if (_weaponConfigs.ContainsKey(weaponConfig.Id))
                    Debug.LogError($"Duplicate: {weaponConfig.Id} | {weaponConfig}");
                else
                    _weaponConfigs.Add(weaponConfig.Id, weaponConfig);
            }
        }
    }
}