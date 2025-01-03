using Fusion;
using UnityEngine;

namespace BlackRed.Game.Bullets
{
    public class NetBulletsSpawner : NetworkBehaviour
    {
        [SerializeField] private NetBulletController netBulletControllerPrefab;
        
        public void Spawn(Vector3 startPosition, Vector3 direction, PlayerRef playerRef) 
            => Runner.Spawn(netBulletControllerPrefab, startPosition, Quaternion.LookRotation(direction), playerRef);
    }
}