using App.Bullets;
using App.PlayerEntities;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players
{
    public class NetPlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private NetBulletsSpawner netBulletsSpawner;
        [SerializeField] private NetPlayerController netPlayerControllerPrefab;

        [Inject] private IEventBus _eventBus;
        
        public NetPlayerController Spawn(PlayerRef playerRef, Transform spawnPoint)
            => Spawn(playerRef, spawnPoint.position, spawnPoint.rotation);
        
        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position)
            => Spawn(playerRef, position, Quaternion.identity);

        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, Quaternion rotation)
        {
            var netPlayerController = Runner.Spawn(netPlayerControllerPrefab, position, rotation, playerRef);
            netPlayerController.Initialize(netBulletsSpawner, _eventBus);

            return netPlayerController;
        }
    }
}