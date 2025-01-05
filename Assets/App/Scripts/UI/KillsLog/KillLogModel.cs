using System;
using App.Entities;
using App.EventBus;
using Avastrad.EventBusFramework;
using Fusion;
using Zenject;

namespace App.UI.KillsLog
{
    public class KillLogModel : NetworkBehaviour, IEventReceiver<OnKill>
    {
        [Inject] private readonly IEventBus _eventBus;
        [Inject] private readonly EntitiesRepository _entitiesRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public event Action<string, string> OnKillLog;

        public override void Spawned()
        {
            _eventBus.Subscribe(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _eventBus.UnSubscribe(this);
        }

        public void OnEvent(OnKill t)
        {
            if (_entitiesRepository.TryGet(t.Killer, out var killer) && _entitiesRepository.TryGet(t.Killed, out var killed))
                Rpc_OnKill(killer.GetName(), killed.GetName());
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true)]
        private void Rpc_OnKill(string killer, string killed) 
            => OnKillLog?.Invoke(killer, killed);
    }
}