using Avastrad.EventBusFramework;
using BlackRed.Game.EventBus;
using BlackRed.Game.Players.SessionDatas;
using Fusion;
using Zenject;

namespace BlackRed.Game.PointsCounting
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