using System;
using App.Entities;
using App.EventBus;
using App.Players.SessionDatas;
using Avastrad.EventBusFramework;
using Fusion;

namespace App.Players.KillsCounting
{
    public class KillsCounter : IEventReceiver<OnKill>, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly PlayerSessionDatasRepository _playerSessionDatasRepository;
        private readonly EntitiesRepository _entitiesRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        public KillsCounter(IEventBus eventBus, PlayerSessionDatasRepository playerSessionDatasRepository, 
            EntitiesRepository entitiesRepository)
        {
            _eventBus = eventBus;
            _playerSessionDatasRepository = playerSessionDatasRepository;
            _entitiesRepository = entitiesRepository;
            
            _eventBus.Subscribe(this);
        }


        private void AddKills(PlayerRef playerRef)
        {
            if (_playerSessionDatasRepository.Datas.TryGetValue(playerRef, out var sessionData))
                sessionData.ChangeKills(1);
        }

        public void OnEvent(OnKill t)
        {
            if (_entitiesRepository.TryGetPlayer(t.Killer, out var player)) 
                AddKills(player.PlayerRef);
        }

        public void Dispose()
        {
            _eventBus.UnSubscribe(this);
        }
    }
}