using App.DisconnectProviding;
using Fusion;
using UnityEngine;
using Zenject;

namespace App
{
    public class NetDisconnecter : NetworkBehaviour
    {
        [Inject] private readonly IDisconnectProvider _disconnectProvider;
        
        public override void Spawned() 
            => _disconnectProvider.OnDisconnectRequest += DisconnectSelf;

        public override void Despawned(NetworkRunner runner, bool hasState) 
            => _disconnectProvider.OnDisconnectRequest -= DisconnectSelf;

        private void DisconnectSelf() 
            => Rpc_DisconnectPlayer(Runner.LocalPlayer);

        [Rpc(RpcSources.All, RpcTargets.StateAuthority, InvokeLocal = true)]
        private void Rpc_DisconnectPlayer(PlayerRef playerRef)
        {
            if (!HasStateAuthority)
            {
                Debug.LogError("You try disconnect local");
                return;
            }

            if (Runner.LocalPlayer == playerRef)
            {
                foreach (var player in Runner.ActivePlayers)
                    if (Object.StateAuthority != player)
                        Runner.Disconnect(player);
                
                Runner.Shutdown();
            }
            else
                Runner.Disconnect(playerRef);
        }
    }
}