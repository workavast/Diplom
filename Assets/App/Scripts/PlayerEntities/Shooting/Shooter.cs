using App.Damage;
using App.Entities;
using App.Particles;
using App.Weapons;
using Fusion;
using UnityEngine;

namespace App.PlayerEntities.Shooting
{
    public class Shooter
    {
        private readonly Transform _shootPoint;
        private readonly WeaponConfig _config;
        private readonly NetParticlesFactory _netParticlesFactory;
        private readonly IDamageApplicator _damageApplicator;
        private readonly IEntity _entity;
        
        private NetworkRunner Runner => _entity.Runner;
        private PlayerRef InputAuthority => _entity.Object.InputAuthority;

        public Shooter(IEntity entity, Transform shootPoint, WeaponConfig config, IDamageApplicator damageApplicator, 
            NetParticlesFactory netParticlesFactory)
        {
            _entity = entity;
            _shootPoint = shootPoint;
            _config = config;
            _damageApplicator = damageApplicator;
            _netParticlesFactory = netParticlesFactory;
        }
        
        public bool Shoot(bool hasStateAuthority, out ProjectileData projectileData)
        {
            var isHit = Runner.LagCompensation.Raycast(_shootPoint.position, _shootPoint.forward, 
                100f, InputAuthority, out var hit, -1, HitOptions.IncludePhysX);
            
            if (isHit)
            {
                if (hit.GameObject == _entity.GameObject) 
                    Debug.LogWarning("Hit it self");

                if (hasStateAuthority)
                    _damageApplicator.TryApplyDamage(_config.DamagePerBullet, hit.GameObject, _entity);

                projectileData = new ProjectileData(hit.Point, hit.Normal);
                return true;
            }
            
            projectileData = default;
            return true;
        }

        public void ShootView(ref ProjectileData projectileData)
        {
            SpawnHitEffect(projectileData.HitPosition, projectileData.HitNormal);
        }
        
        private void SpawnHitEffect(Vector3 hitPoint, Vector3 normal) 
            => _netParticlesFactory.SpawnParticleEffect(ParticleType.BulletCollision, hitPoint, normal);

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(_shootPoint.position, _shootPoint.position + _shootPoint.forward * 100f);
        }
    }
}