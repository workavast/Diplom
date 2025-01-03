using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players.SessionDatas
{
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetPlayerReSpawner))]
    public class NetPlayerSessionData : NetworkBehaviour
    {
        [Networked] [HideInInspector] 
        [OnChangedRender(nameof(NickNameChanged))] 
        public NetworkString<_16> NickName { get; private set; }

        [Networked] [HideInInspector] 
        [OnChangedRender(nameof(PointsChanged))] 
        public int Points { get; private set; }
        
        [Networked] [HideInInspector] 
        [OnChangedRender(nameof(DeathsChanged))] 
        public int Deaths { get; private set; }
        
        [Networked] [HideInInspector] 
        [OnChangedRender(nameof(KillsChanged))] 
        public int Kills { get; private set; }

        private IPlayerSessionDatasRepository _playerSessionDatasRepository;
        
        public event Action OnDespawned;
        public event Action OnNickNameChanged;
        public event Action OnPointsChanged;
        public event Action OnKillsChanged;
        public event Action OnDeathsChanged;

        [Inject]
        public void Construct(IPlayerSessionDatasRepository playerSessionDatasRepository)
        {
            _playerSessionDatasRepository = playerSessionDatasRepository;
        }
        
        public void Initialize(PlayerSpawnPointsProvider playerSpawnPointsProvider, NetPlayerSpawner playerSpawner)
        {
            GetComponent<NetPlayerReSpawner>().Initialize(playerSpawnPointsProvider, playerSpawner);
        }
        
        public override void Spawned()
        {
            base.Spawned();

            var bink = NickName.ToString();
            if (HasInputAuthority)
            {
                if (HasStateAuthority)
                    NickName = PlayerData.NickName;
                else
                    Rpc_SetNickName(PlayerData.NickName);

                if (Object.HasStateAuthority)
                    Points = 0;
            }

            _playerSessionDatasRepository.Add(Object.InputAuthority, this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            _playerSessionDatasRepository.Remove(Object.InputAuthority);
            OnDespawned?.Invoke();
        }
        
        private void NickNameChanged() 
            => OnNickNameChanged?.Invoke();

        private void PointsChanged() 
            => OnPointsChanged?.Invoke();
        
        private void KillsChanged() 
            => OnKillsChanged?.Invoke();

        private void DeathsChanged() 
            => OnDeathsChanged?.Invoke();

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void Rpc_SetNickName(string nickName)
        {
            NickName = nickName;
        }

        public void ChangePoints(int value)
        {
            Points += value;
        }
        
        public void ChangeKills(int value)
        {
            Kills += value;
        }
        
        public void ChangeDeaths(int value)
        {
            Deaths += value;
        }
    }
}