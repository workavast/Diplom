using App.EventBus;
using App.Players.SessionDatas;
using Avastrad.EventBusFramework;
using Fusion;
using Zenject;

namespace App.PointsCounting
{
    public class NetPointsCounter : NetworkBehaviour, IEventReceiver<OnPlayerKill>
    {
        [Inject] private IEventBus _eventBus;
        [Inject] private IPlayerSessionDatasRepository _playerSessionDatasRepository;

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

        public void OnEvent(OnPlayerKill t) 
            => AddPoints(t.Killer);
    }
}