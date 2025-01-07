using App.Damage;
using App.Entities;
using App.Particles;
using App.Weapons;
using UnityEngine;

namespace App.PlayerEntities.Shooting
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
        
        public Shooter CreateShoot(IEntity entity, Transform shootPoint, WeaponConfig config)
        {
            var damageApplicator = _damageApplicatorFactory.CreateDamageApplicator(entity.EntityType);
            return new Shooter(entity, shootPoint, config, damageApplicator, _netParticlesFactory);
        }
    }
}