using App.PlayerEntities.Shooting;

namespace App.Weapons
{
    public class Weapon
    {
        private readonly WeaponConfig _weaponConfig;
        private readonly Shooter _shooter;
        
        public Weapon(WeaponConfig weaponConfig, Shooter shooter)
        {
            _weaponConfig = weaponConfig;
            _shooter = shooter;
        }

        public void Shoot(bool hasStateAuthority) 
            => _shooter.Shoot(hasStateAuthority);

        public void OnDrawGizmos() 
            => _shooter.OnDrawGizmos();
    }
}