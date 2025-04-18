using System;
using App.Entities;
using App.EventBus;
using App.Players.SessionData;
using App.Pvp.Gameplay;
using Avastrad.EventBusFramework;
using Fusion;

namespace App.Pvp.KillsCounting
{
    public class KillsCounter : IEventReceiver<OnKill>, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly EntitiesRepository _entitiesRepository;
        private readonly IPlayersSessionDataRepository<NetGameplaySessionData> _gameplaySessionDataRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public KillsCounter(IEventBus eventBus, EntitiesRepository entitiesRepository, 
            IPlayersSessionDataRepository<NetGameplaySessionData> gameplaySessionDataRepository)
        {
            _eventBus = eventBus;
            _entitiesRepository = entitiesRepository;
            _gameplaySessionDataRepository = gameplaySessionDataRepository;

            _eventBus.Subscribe(this);
        }


        private void AddKills(PlayerRef playerRef)
        {
            if (_gameplaySessionDataRepository.ContainsKey(playerRef))
                _gameplaySessionDataRepository.GetData(playerRef).ChangeKills(1);
        }

        public void OnEvent(OnKill e)
        {
            if (_entitiesRepository.TryGetPlayer(e.Killer, out var player)) 
                AddKills(player.PlayerRef);
        }

        public void Dispose()
        {
            _eventBus.UnSubscribe(this);
        }
    }
}