using App.Entities;
using App.EventBus;
using App.Players.SessionDatas;
using Avastrad.EventBusFramework;
using Fusion;
using Zenject;

namespace App.PointsCounting
{
    public class NetPointsCounter : NetworkBehaviour, IEventReceiver<OnKill>
    {
        [Inject] private readonly IEventBus _eventBus;
        [Inject] private readonly IPlayerSessionDatasRepository _playerSessionDatasRepository;
        [Inject] private readonly EntitiesRepository _entitiesRepository;
        
        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new EventBusReceiverIdentifier();

        private void Awake()
        {
            _eventBus.Subscribe(this);
        }

        private void AddPoints(PlayerRef playerRef)
        {
            if (_playerSessionDatasRepository.Datas.TryGetValue(playerRef, out var sessionData))
                sessionData.ChangePoints(1);
        }

        public void OnEvent(OnKill t)
        {
            if (_entitiesRepository.TryGetPlayer(t.Killer, out var player)) 
                AddPoints(player.PlayerRef);
        }
    }
}