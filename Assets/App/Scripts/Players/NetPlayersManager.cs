using System.Collections.Generic;
using App.DisconnectProviding;
using App.Players.Repository;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players
{
    public class NetPlayersManager : NetworkBehaviour
    {
        [SerializeField] private NetPlayerSpawner playerSpawner;
        [SerializeField] private NetPlayerReSpawner playerReSpawnerPrefab;
        [SerializeField] private PlayerSpawnPointsProvider playerSpawnPointsProvider;
        
        [Inject] private readonly IDisconnectProvider _disconnectProvider;
        [Inject] private readonly IReadOnlyPlayersRepository _playersRepository;
        
        private readonly Dictionary<PlayerRef, NetPlayerReSpawner> _playerReSpawners = new(Consts.MaxPlayersCount);

        public override void Spawned()
        {
            _disconnectProvider.OnDisconnectRequest += DisconnectPlayer;

            foreach (var activePlayer in _playersRepository.Players)
            {
                if (!_playerReSpawners.ContainsKey(activePlayer))
                    CreateNetPlayerReSpawner(activePlayer);
            }

            _playersRepository.OnPlayerJoined += CreateNetPlayerReSpawner;
            _playersRepository.OnPlayerLeft += DeleteSessionData;
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersRepository.OnPlayerJoined -= CreateNetPlayerReSpawner;
            _playersRepository.OnPlayerLeft -= DeleteSessionData;
            _disconnectProvider.OnDisconnectRequest -= DisconnectPlayer;
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

        private void DeleteSessionData(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;
            
            if (_playerReSpawners.TryGetValue(player, out var data))
            {
                Runner.Despawn(data.GetComponent<NetworkObject>());
                _playerReSpawners.Remove(player);
            }
        }

        private void DisconnectPlayer() 
            => Rpc_DisconnectPlayer(Runner.LocalPlayer);

        [Rpc(RpcSources.All, RpcTargets.StateAuthority, InvokeLocal = true)]
        private void Rpc_DisconnectPlayer(PlayerRef playerRef)
        {
            if (!HasStateAuthority) 
                return;
            
            if (Runner.LocalPlayer == playerRef)
            {
                foreach (var player in _playerReSpawners.Keys)
                    if (Object.StateAuthority != player)
                        Runner.Disconnect(player);

                Runner.Shutdown();
            }
            else
                Runner.Disconnect(playerRef);
        }
    }
}
