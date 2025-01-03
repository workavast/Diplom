using System;
using App.EventBus;
using App.Players.SessionDatas;
using Avastrad.EventBusFramework;
using Fusion;

namespace App.Players.DeathsCounting
{
    public class DeathsCounter : IEventReceiver<OnPlayerKill>, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly PlayerSessionDatasRepository _playerSessionDatasRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public DeathsCounter(IEventBus eventBus, PlayerSessionDatasRepository playerSessionDatasRepository)
        {
            _eventBus = eventBus;
            _playerSessionDatasRepository = playerSessionDatasRepository;
            _eventBus.Subscribe(this);
        }
        
        private void AddDeaths(PlayerRef playerRef)
        {
            if (_playerSessionDatasRepository.Datas.TryGetValue(playerRef, out var sessionData))
                sessionData.ChangeDeaths(1);
        }

        public void OnEvent(OnPlayerKill t) 
            => AddDeaths(t.KilledPlayer);

        public void Dispose()
        {
            _eventBus.UnSubscribe(this);
        }
    }
}