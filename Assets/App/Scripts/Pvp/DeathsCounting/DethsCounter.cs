using System;
using App.Entities;
using App.EventBus;
using App.Players.SessionData;
using App.Pvp.Gameplay;
using Avastrad.EventBusFramework;
using Fusion;

namespace App.Pvp.DeathsCounting
{
    public class DeathsCounter : IEventReceiver<OnKill>, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly EntitiesRepository _entitiesRepository;
        private readonly IPlayersSessionDataRepository<NetGameplaySessionData> _gameplaySessionDataRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public DeathsCounter(IEventBus eventBus, EntitiesRepository entitiesRepository, 
            IPlayersSessionDataRepository<NetGameplaySessionData> gameplaySessionDataRepository)
        {
            _eventBus = eventBus;
            _entitiesRepository = entitiesRepository;
            _gameplaySessionDataRepository = gameplaySessionDataRepository;

            _eventBus.Subscribe(this);
        }
        
        private void AddDeaths(PlayerRef playerRef)
        {
            if (_gameplaySessionDataRepository.ContainsKey(playerRef))
                _gameplaySessionDataRepository.GetData(playerRef).ChangeDeaths(1);
        }

        public void OnEvent(OnKill e)
        {
            if (_entitiesRepository.TryGetPlayer(e.Killed, out var player)) 
                AddDeaths(player.PlayerRef);
        }

        public void Dispose()
        {
            _eventBus.UnSubscribe(this);
        }
    }
}