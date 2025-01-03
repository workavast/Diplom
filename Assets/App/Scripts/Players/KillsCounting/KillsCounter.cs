using System;
using Avastrad.EventBusFramework;
using BlackRed.Game.EventBus;
using BlackRed.Game.Players.SessionDatas;
using Fusion;

namespace BlackRed.Game.Players.KillsCounting
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