using BlackRed.Game.ParticlesSpawning;
using BlackRed.Game.PlayerEntities;
using Fusion;
using UnityEngine;

namespace BlackRed.Game.Bullets
{
    [RequireComponent(typeof(BulletView))]
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetBulletCollisionProvider))]
    public class NetBulletController : NetworkBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; } = 2.5f;
        [field: SerializeField] public int Damage { get; private set; } = 10;

        private BulletView _bulletView;
        private NetBulletCollisionProvider _netBulletCollisionProvider;
        private NetParticlesSpawner _netParticlesSpawner;
        
        public PlayerRef PlayerRef => Object.InputAuthority;

        private void Awake()
        {
            _bulletView = GetComponent<BulletView>();
            _netBulletCollisionProvider = GetComponent<NetBulletCollisionProvider>();
            
            _netParticlesSpawner = FindObjectOfType<NetParticlesSpawner>();
        }

        public override void Spawned()
        {
            _netBulletCollisionProvider.OnCollide += OnCollision;
            _netBulletCollisionProvider.OnCollidePlayer += OnCollisionPlayer;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _netBulletCollisionProvider.OnCollide -= OnCollision;
            _netBulletCollisionProvider.OnCollidePlayer -= OnCollisionPlayer;
        }

        public override void FixedUpdateNetwork()
        {
            _bulletView.Move(Speed, Runner.DeltaTime);
        }

        private void OnCollision(Vector3 hitPoint, Vector3 normal)
        {
            if (HasInputAuthority)
                _netParticlesSpawner.SpawnParticleEffect(ParticleType.BulletCollision, hitPoint, normal);
            
            if (HasStateAuthority)
                Runner.Despawn(GetComponent<NetworkObject>());
        }

        private void OnCollisionPlayer(NetPlayerController netPlayerController, Vector3 hitPoint, Vector3 normal)
        {
            if (netPlayerController.PlayerRef == Object.InputAuthority)
                return;

            netPlayerController.TakeDamage(Damage, Object.InputAuthority);

            OnCollision(hitPoint, normal);
        }
    }
}