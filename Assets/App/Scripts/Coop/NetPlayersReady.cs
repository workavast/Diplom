using System;
using System.Linq;
using Fusion;
using UnityEngine;

namespace App.Coop
{
    public class NetPlayersReady : NetworkBehaviour
    {
        public bool AllPlayersIsReady { get; private set; }
        
        private int _readyCounter;
        private bool _isReadySend;

        public event Action<PlayerRef> OnPlayerIsReady;
        public event Action OnAllPlayersIsReady;

        public override void Spawned()
        {
            Runner.SetIsSimulated(Object, true);
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
            
            if (AllPlayersIsReady)
                return;

            _readyCounter++;
            if (_readyCounter >= Runner.ActivePlayers.Count())
            {
                AllPlayersIsReady = true;
                OnAllPlayersIsReady?.Invoke();
            }
        }
    }
}