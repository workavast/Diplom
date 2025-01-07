using App.Entities;
using App.PlayerEntities.Shooting;
using UnityEngine;

namespace App.Weapons
{
    public class WeaponFactory
    {
        private readonly WeaponsConfigs _weaponsConfigs;
        private readonly ShooterFactory _shooterFactory;

        public WeaponFactory(WeaponsConfigs weaponsConfigs, ShooterFactory shooterFactory)
        {
            _weaponsConfigs = weaponsConfigs;
            _shooterFactory = shooterFactory;
        }
        
        public Weapon Create(WeaponId weaponId, IEntity entity, Transform shootPoint)
        {
            var config = _weaponsConfigs.WeaponConfigs[weaponId];
            var shooter = _shooterFactory.CreateShoot(entity, shootPoint, config);
            
            return new Weapon(config, shooter);
        }
    }
}