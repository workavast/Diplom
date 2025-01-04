using App.PlayerEntities;
using Fusion;
using UnityEngine;

namespace App.Players
{
    public class NetPlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private NetPlayerController netPlayerControllerPrefab;

        public NetPlayerController Spawn(PlayerRef playerRef, Transform spawnPoint)
            => Spawn(playerRef, spawnPoint.position, spawnPoint.rotation);
        
        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position)
            => Spawn(playerRef, position, Quaternion.identity);

        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, Quaternion rotation)
        {
            var netPlayerController = Runner.Spawn(netPlayerControllerPrefab, position, rotation, playerRef);
            return netPlayerController;
        }
    }
}