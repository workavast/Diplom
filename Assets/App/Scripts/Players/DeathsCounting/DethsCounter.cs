using System;
using Avastrad.EventBusFramework;
using BlackRed.Game.EventBus;
using BlackRed.Game.Players.SessionDatas;
using Fusion;

namespace BlackRed.Game.Players.DeathsCounting
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