using System;
using Avastrad.EventBusFramework;
using BlackRed.Game.EventBus;
using Fusion;
using Zenject;

namespace BlackRed.Game.UI.KillsLog
{
    public class KillLogModel : NetworkBehaviour, IEventReceiver<OnPlayerKill>
    {
        [Inject] private IEventBus _eventBus;
        
        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public event Action<PlayerRef, PlayerRef> OnKillLog;

        public override void Spawned()
        {
            _eventBus.Subscribe(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _eventBus.UnSubscribe(this);
        }

        public void OnEvent(OnPlayerKill t) 
            => Rpc_OnKill(t.Killer, t.KilledPlayer);

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true)]
        private void Rpc_OnKill(PlayerRef killer, PlayerRef killed) 
            => OnKillLog?.Invoke(killer, killed);
    }
}