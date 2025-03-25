using System.Collections.Generic;
using App.Coop;
using App.Players;
using App.Players.Repository;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Pvp
{
    public class NetPlayersManager : NetworkBehaviour
    {
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private NetPlayerReSpawner playerReSpawnerPrefab;
        [SerializeField] private PlayerSpawnPointsProvider playerSpawnPointsProvider;
        [SerializeField] private NetPlayersReady playersReady;

        [Inject] private readonly IReadOnlyPlayersRepository _playersRepository;
        
        private readonly Dictionary<PlayerRef, NetPlayerReSpawner> _playerReSpawners = new(Consts.MaxPlayersCount);

        public override void Spawned()
        {
            playersReady.OnPlayerIsReady += TryCreateNetPlayerReSpawner;
            _playersRepository.OnPlayerLeft += DeleteReSpawners;
            
            if (playersReady.AllPlayersIsReady) 
                InitializeSpawning();
            else
                playersReady.OnAllPlayersIsReady += InitializeSpawning;
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersRepository.OnPlayerJoined -= CreateNetPlayerReSpawner;
            _playersRepository.OnPlayerLeft -= DeleteReSpawners;
        }

        private void TryCreateNetPlayerReSpawner(PlayerRef playerRef)
        {
            if (playersReady.AllPlayersIsReady) 
                CreateNetPlayerReSpawner(playerRef);
        }
        
        private void InitializeSpawning()
        {
            foreach (var activePlayer in _playersRepository.Players)
            {
                if (!_playerReSpawners.ContainsKey(activePlayer))
                    CreateNetPlayerReSpawner(activePlayer);
            }
        }

        private void CreateNetPlayerReSpawner(PlayerRef playerRef)
        {
            if (!HasStateAuthority)
                return;

            Debug.Log($"Create NetPlayerReSpawner {playerRef.PlayerId} | {playerRef}");
            if (_playerReSpawners.ContainsKey(playerRef))
            {
                Debug.LogWarning("player already joined");
                return;
            }

            var netPlayerSessionData = Runner.Spawn(playerReSpawnerPrefab, Vector3.zero, Quaternion.identity, playerRef);
            netPlayerSessionData.Initialize(playerSpawnPointsProvider, playerSpawner);
            _playerReSpawners.Add(playerRef, netPlayerSessionData);
        }

        private void DeleteReSpawners(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;
            
            if (_playerReSpawners.TryGetValue(player, out var data))
            {
                Runner.Despawn(data.GetComponent<NetworkObject>());
                _playerReSpawners.Remove(player);
            }
        }
    }
}
