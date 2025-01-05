using System;
using App.Entities;
using App.EventBus;
using App.Players.SessionDatas;
using Avastrad.EventBusFramework;
using Fusion;

namespace App.Players.DeathsCounting
{
    public class DeathsCounter : IEventReceiver<OnKill>, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly PlayerSessionDatasRepository _playerSessionDatasRepository;
        private readonly EntitiesRepository _entitiesRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public DeathsCounter(IEventBus eventBus, PlayerSessionDatasRepository playerSessionDatasRepository, 
            EntitiesRepository entitiesRepository)
        {
            _eventBus = eventBus;
            _playerSessionDatasRepository = playerSessionDatasRepository;
            _entitiesRepository = entitiesRepository;
            
            _eventBus.Subscribe(this);
        }
        
        private void AddDeaths(PlayerRef playerRef)
        {
            if (_playerSessionDatasRepository.Datas.TryGetValue(playerRef, out var sessionData))
                sessionData.ChangeDeaths(1);
        }

        public void OnEvent(OnKill t)
        {
            if (_entitiesRepository.TryGetPlayer(t.Killed, out var player)) 
                AddDeaths(player.PlayerRef);
        }

        public void Dispose()
        {
            _eventBus.UnSubscribe(this);
        }
    }
}