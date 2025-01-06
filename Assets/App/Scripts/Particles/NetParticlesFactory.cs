using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace App.Particles
{
    public class NetParticlesFactory : NetworkBehaviour
    {
        [SerializeField] private ParticleFactory particleFactory;

        private readonly Dictionary<byte, ParticleType> _particleTypes =new()
        {
            {1, ParticleType.BulletCollision}
        };
        private readonly Dictionary<ParticleType, byte> _particleBytes =new()
        {
            {ParticleType.BulletCollision, 1}
        };
        
        public void SpawnParticleEffect(ParticleType particleType, Vector3 point, Vector3 normal) 
            => Rpc_SpawnParticleEffect(_particleBytes[particleType], point, normal);

        private void Spawn(byte particleByte, Vector3 point, Vector3 normal)
            => particleFactory.Create(_particleTypes[particleByte], point, Quaternion.LookRotation(normal));

        // [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        private void Rpc_SpawnParticleEffect(byte particleByte, Vector3 point, Vector3 normal) 
            => Spawn(particleByte, point, normal);
    }
}