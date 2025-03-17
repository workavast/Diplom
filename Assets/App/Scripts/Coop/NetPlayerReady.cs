using System;
using System.Linq;
using Fusion;
using UnityEngine;

namespace App.Coop
{
    public class NetPlayerReady : NetworkBehaviour
    {
        public bool AllPlayersIsReady { get; private set; }
        private int _readyCounter;

        private bool _isReadySend;
        
        public event Action OnAllPlayersIsReady;

        public override void FixedUpdateNetwork()
        {
            if (_isReadySend)
                return;

            if (HasStateAuthority || HasInputAuthority)
            {
                _isReadySend = true;
                RPC_PlayerIsReady();
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority, InvokeLocal = true)]
        private void RPC_PlayerIsReady()
        {
            if (!HasStateAuthority)
            {
                Debug.LogError("Doesnt have state authority");
                return;
            }

            if (AllPlayersIsReady)
            {
                Debug.LogError("All players already ready");
                return;
            }
            
            _readyCounter++;
            if (_readyCounter >= Runner.ActivePlayers.Count())
            {
                AllPlayersIsReady = true;
                OnAllPlayersIsReady?.Invoke();
            }
        }
    }
}