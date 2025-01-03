using System;
using App.EventBus;
using App.Players.SessionDatas;
using Avastrad.EventBusFramework;
using Fusion;

namespace App.Players.KillsCounting
{
    public class KillsCounter : IEventReceiver<OnPlayerKill>, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly PlayerSessionDatasRepository _playerSessionDatasRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public KillsCounter(IEventBus eventBus, PlayerSessionDatasRepository playerSessionDatasRepository)
        {
            _eventBus = eventBus;
            _playerSessionDatasRepository = playerSessionDatasRepository;
            _eventBus.Subscribe(this);
        }


        private void AddKills(PlayerRef playerRef)
        {
            if (_playerSessionDatasRepository.Datas.TryGetValue(playerRef, out var sessionData))
                sessionData.ChangeKills(1);
        }

        public void OnEvent(OnPlayerKill t) 
            => AddKills(t.Killer);

        public void Dispose()
        {
            _eventBus.UnSubscribe(this);
        }
    }
}