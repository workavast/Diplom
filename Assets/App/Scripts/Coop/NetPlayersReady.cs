using System;
using System.Linq;
using Fusion;
using UnityEngine;

namespace App.Coop
{
    public class NetPlayersReady : NetworkBehaviour
    {
        [Networked][OnChangedRender(nameof(AllPlayersBecameReady))][field: SerializeField, ReadOnly] private bool allPlayersIsReady { get; set; }
        
        public bool AllPlayersIsReady => _isInitialized && allPlayersIsReady;
        
        private bool _isInitialized;
        private int _readyCounter;
        private bool _isReadySend;

        public event Action<PlayerRef> OnPlayerIsReady;
        public event Action OnAllPlayersIsReady;

        public override void Spawned()
        {
            _isInitialized = true;
            Runner.SetIsSimulated(Object, true);

            if (allPlayersIsReady) 
                OnAllPlayersIsReady?.Invoke();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _isInitialized = false;
        }

        public override void FixedUpdateNetwork()
        {
            if (_isReadySend)
                return;

            if (!Runner.IsResimulation)
            {
                _isReadySend = true;
                RPC_PlayerIsReady(Runner.LocalPlayer);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_PlayerIsReady(PlayerRef playerRef)
        {
            if (!HasStateAuthority)
            {
                Debug.LogError("Doesnt have state authority");
                return;
            }

            OnPlayerIsReady?.Invoke(playerRef);
            
            if (allPlayersIsReady)
                return;

            _readyCounter++;
            if (_readyCounter >= Runner.ActivePlayers.Count()) 
                allPlayersIsReady = true;
        }

        private void AllPlayersBecameReady()
        {
            OnAllPlayersIsReady?.Invoke();
        }
    }
}