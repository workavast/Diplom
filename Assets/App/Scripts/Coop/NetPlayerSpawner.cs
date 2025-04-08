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
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;

        private bool _gameIsInitialized;

        public override void Spawned()
        {
            if (!HasStateAuthority)
                return;

            playersReady.OnPlayerIsReady += TrySpawnPlayer;
            _playersRepository.OnPlayerLeft += DespawnPlayerEntity;
            
            if (playersReady.AllPlayersIsReady) 
                SpawnPlayers();
            else
                playersReady.OnAllPlayersIsReady += SpawnPlayers;
        }

        private void TrySpawnPlayer(PlayerRef playerRef)
        {
            if(playersReady.AllPlayersIsReady)
                SpawnPlayer(playerRef);
        }

        private void DespawnPlayerEntity(PlayerRef playerRef)
        {
            if (_playersEntitiesRepository.TryGet(playerRef, out var entity)) 
                Runner.Despawn(entity.Object);
        }
        
        private void SpawnPlayers()
        {
            foreach (var player in _playersRepository.Players) 
                SpawnPlayer(player);
        }
        
        private void SpawnPlayer(PlayerRef playerRef)
        {
            var weaponId = _coopSessionDataRepository.GetData(playerRef).SelectedWeapon;
            var armorLevel = _coopSessionDataRepository.GetData(playerRef).EquippedArmorLevel;
            playerSpawner.Spawn(playerRef, playerSpawnPointsProvider.SpawnPoints[playerRef.PlayerId - 1], armorLevel, weaponId);
        }
    }
}