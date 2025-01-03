using App.PlayerEntities;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players
{
    public class NetPlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private NetPlayerController netPlayerControllerPrefab;

        [Inject] private IEventBus _eventBus;
        
        public NetPlayerController Spawn(PlayerRef playerRef, Transform spawnPoint)
            => Spawn(playerRef, spawnPoint.position, spawnPoint.rotation);
        
        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position)
            => Spawn(playerRef, position, Quaternion.identity);

        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, Quaternion rotation)
        {
            var netPlayerController = Runner.Spawn(netPlayerControllerPrefab, position, rotation, playerRef);
            netPlayerController.Initialize(_eventBus);

            return netPlayerController;
        }
    }
}