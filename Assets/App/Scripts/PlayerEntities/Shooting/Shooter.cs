using App.Damage;
using App.Entities;
using App.Particles;
using Fusion;
using UnityEngine;

namespace App.PlayerEntities.Shooting
{
    public class Shooter
    {
        private readonly Transform _shootPoint;
        private readonly PlayerEntityConfig _config;
        private readonly NetParticlesFactory _netParticlesFactory;
        private readonly IDamageApplicator _damageApplicator;
        private readonly IEntity _entity;
        
        private NetworkRunner Runner => _entity.Runner;
        private PlayerRef InputAuthority => _entity.Object.InputAuthority;

        public Shooter(IEntity entity, Transform shootPoint, PlayerEntityConfig config, IDamageApplicator damageApplicator, 
            NetParticlesFactory netParticlesFactory)
        {
            _entity = entity;
            _shootPoint = shootPoint;
            _config = config;
            _damageApplicator = damageApplicator;
            _netParticlesFactory = netParticlesFactory;
        }
        
        public void Shoot(bool hasStateAuthority)
        {
            var isHit = Runner.LagCompensation.Raycast(_shootPoint.position, _shootPoint.forward, 
                100f, InputAuthority, out var hit);
            
            if (isHit)
            {
                if (hit.GameObject == _entity.GameObject) 
                    Debug.LogWarning("Hit it self");

                if (hasStateAuthority)
                    _damageApplicator.TryApplyDamage(_config.Damage, hit.GameObject, _entity);
                
                SpawnHitEffect(hit.Point, hit.Normal);
            }
        }
        
        private void SpawnHitEffect(Vector3 hitPoint, Vector3 normal) 
            => _netParticlesFactory.SpawnParticleEffect(ParticleType.BulletCollision, hitPoint, normal);

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(_shootPoint.position, _shootPoint.position + _shootPoint.forward * 100f);
        }
    }
}