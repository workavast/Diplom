using App.Damage;
using App.Entities;
using App.Particles;

namespace App.Weapons.Shooting
{
    public class ShooterFactory
    {
        private readonly DamageApplicatorFactory _damageApplicatorFactory;
        private readonly NetParticlesFactory _netParticlesFactory;

        public ShooterFactory(DamageApplicatorFactory damageApplicatorFactory, NetParticlesFactory netParticlesFactory)
        {
            _damageApplicatorFactory = damageApplicatorFactory;
            _netParticlesFactory = netParticlesFactory;
        }
        
        public Shooter CreateShoot(IEntity entity)
        {
            var damageApplicator = _damageApplicatorFactory.CreateDamageApplicator(entity.EntityType);
            return new Shooter(entity, damageApplicator, _netParticlesFactory);
        }
    }
}