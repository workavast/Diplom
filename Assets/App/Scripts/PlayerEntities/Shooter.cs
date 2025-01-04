using App.Damage;
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

        private void Start()
        {
            _netParticlesSpawner = FindFirstObjectByType<NetParticlesSpawner>();
        }

        public void Shoot()
        {
            var isHit = Runner.LagCompensation.Raycast(shootPoint.position, shootPoint.forward, 
                100f, Object.InputAuthority, out var hit);
            
            if (isHit)
            {
                var netPlayerController = hit.GameObject.GetComponent<NetPlayerController>();

                if (netPlayerController != null)
                    TryDamagePlayer(netPlayerController, hit.Point, hit.Normal);
                else
                    SpawnHitEffect(hit.Point, hit.Normal);
            }
        }

        public void ShootView()
        {
            var isHit = Runner.LagCompensation.Raycast(shootPoint.position, shootPoint.forward, 
                100f, Object.InputAuthority, out var hit);
            
            if (isHit) 
                _netParticlesSpawner.SpawnParticleEffect(ParticleType.BulletCollision, hit.Point, hit.Normal);
        }
        
        private void SpawnHitEffect(Vector3 hitPoint, Vector3 normal)
        {
            if (HasInputAuthority)
                _netParticlesSpawner.SpawnParticleEffect(ParticleType.BulletCollision, hitPoint, normal);
        }

        private void TryDamagePlayer(NetPlayerController netPlayerController, Vector3 hitPoint, Vector3 normal)
        {
            if (netPlayerController.PlayerRef == Object.InputAuthority)
                return;

            _damageApplicator.TryApplyDamage(config.Damage, netPlayerController, Object.InputAuthority);

            SpawnHitEffect(hitPoint, normal);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * 100f);
        }
    }
}