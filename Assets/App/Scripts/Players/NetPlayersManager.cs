using System;
using System.Collections.Generic;
using App.DisconnectProviding;
using App.NetworkRunning;
using App.Players.SessionDatas;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players
{
    public class NetPlayersManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] private NetPlayerSpawner playerSpawner;
        [SerializeField] private NetPlayerSessionData playerSessionDataPrefab;
        [SerializeField] private PlayerSpawnPointsProvider playerSpawnPointsProvider;
        
        [Inject] private IDisconnectProvider _disconnectProvider;
        [Inject] private NetworkRunnerProvider _networkRunnerProvider;
        
        private readonly Dictionary<PlayerRef, NetPlayerSessionData> _playerSessionDatas = new(2);

        public event Action OnPlayerJoined;
        public event Action OnPlayerLeft;

        public override void Spawned()
        {
            _disconnectProvider.OnDisconnectRequest += DisconnectPlayer;
            
            foreach (var activePlayer in Runner.ActivePlayers) 
                PlayerJoined(activePlayer);
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _disconnectProvider.OnDisconnectRequest -= DisconnectPlayer;
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;

            var netPlayerSessionData = Runner.Spawn(playerSessionDataPrefab, Vector3.zero, Quaternion.identity, player);
            netPlayerSessionData.Initialize(playerSpawnPointsProvider, playerSpawner);
            _playerSessionDatas.Add(player, netPlayerSessionData);
            
            OnPlayerJoined?.Invoke();
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;
            
            if (_playerSessionDatas.TryGetValue(player, out var data))
            {
                Runner.Despawn(data.GetComponent<NetworkObject>());
                _playerSessionDatas.Remove(player);
            }
            
            OnPlayerLeft?.Invoke();
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
                foreach (var player in _playerSessionDatas.Keys)
                    if (Object.StateAuthority != player)
                        Runner.Disconnect(player);

                Runner.Shutdown();
            }
            else
                Runner.Disconnect(playerRef);
        }
    }
}
