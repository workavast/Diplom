using App.Damage;
using App.Entities;
using App.ParticlesSpawning;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerEntities
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private PlayerEntityConfig config;

        [Inject] private IDamageApplicator _damageApplicator;
        
        private NetParticlesSpawner _netParticlesSpawner;
        private IEntity _entity;
        
        private void Start()
        {
            _netParticlesSpawner = FindFirstObjectByType<NetParticlesSpawner>();
            _entity = GetComponent<IEntity>();
        }

        public void Shoot(bool hasStateAuthority)
        {
            var isHit = Runner.LagCompensation.Raycast(shootPoint.position, shootPoint.forward, 
                100f, Object.InputAuthority, out var hit);
            
            if (isHit)
            {
                if (hit.GameObject == gameObject) 
                    Debug.LogWarning("Hit it self");

                if (hasStateAuthority)
                    _damageApplicator.TryApplyDamage(config.Damage, hit.GameObject, _entity);
                
                SpawnHitEffect(hit.Point, hit.Normal);
            }
        }
        
        private void SpawnHitEffect(Vector3 hitPoint, Vector3 normal) 
            => _netParticlesSpawner.SpawnParticleEffect(ParticleType.BulletCollision, hitPoint, normal);

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * 100f);
        }
    }
}