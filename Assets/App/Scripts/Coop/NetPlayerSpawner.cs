using App.Coop.Gameplay;
using App.Entities.Player;
using App.Players;
using App.Players.Repository;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Coop
{
    public class NetPlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerSpawnPointsProvider playerSpawnPointsProvider;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private NetPlayersReady playersReady;

        [Inject] private readonly IReadOnlyPlayersRepository _playersRepository;
        [Inject] private readonly CoopSessionDataRepository _coopSessionDataRepository;
        
        private bool _gameIsInitialized;
        private NetPlayerController _netPlayerController;

        public override void Spawned()
        {
            if (!HasStateAuthority)
                return;
            
            if (playersReady.AllPlayersIsReady) 
                SpawnPlayers();
            else
                playersReady.OnAllPlayersIsReady += SpawnPlayers;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (_netPlayerController != null) 
                runner.Despawn(_netPlayerController.Object);
        }

        private void SpawnPlayers()
        {
            foreach (var player in _playersRepository.Players) 
                SpawnPlayer(player);
        }
        
        private void SpawnPlayer(PlayerRef playerRef)
        {
            var weaponId = _coopSessionDataRepository.GetData(playerRef).SelectedWeapon;
            _netPlayerController = playerSpawner.Spawn(playerRef, playerSpawnPointsProvider.GetRandomFreeSpawnPoint(),
                weaponId);
        }
    }
}