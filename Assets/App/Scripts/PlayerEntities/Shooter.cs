using App.ParticlesSpawning;
using Fusion;
using UnityEngine;

namespace App.PlayerEntities
{
    public class Shooter : NetworkBehaviour
    {
        [SerializeField] private Transform shootPoint;
        [field: SerializeField] public int Damage { get; private set; } = 10;
        
        private NetParticlesSpawner _netParticlesSpawner;

        private void Awake()
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
                    OnCollisionPlayer(netPlayerController, hit.Point, hit.Normal);
                else
                    OnCollision(hit.Point, hit.Normal);
            }
        }
        
        private void OnCollision(Vector3 hitPoint, Vector3 normal)
        {
            if (HasInputAuthority)
                _netParticlesSpawner.SpawnParticleEffect(ParticleType.BulletCollision, hitPoint, normal);
        }

        private void OnCollisionPlayer(NetPlayerController netPlayerController, Vector3 hitPoint, Vector3 normal)
        {
            if (netPlayerController.PlayerRef == Object.InputAuthority)
                return;

            netPlayerController.TakeDamage(Damage, Object.InputAuthority);

            OnCollision(hitPoint, normal);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * 100f);
        }
    }
}