using App.Entities;
using App.EventBus;
using App.Players.SessionData;
using App.Pvp.Gameplay;
using Avastrad.EventBusFramework;
using Fusion;
using Zenject;

namespace App.PointsCounting
{
    public class NetPointsCounter : NetworkBehaviour, IEventReceiver<OnKill>
    {
        [Inject] private readonly IEventBus _eventBus;
        [Inject] private readonly EntitiesRepository _entitiesRepository;
        [Inject] private readonly IPlayersSessionDataRepository<NetGameplaySessionData> _gameplaySessionDataRepository;

        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new EventBusReceiverIdentifier();

        private void Awake()
        {
            _eventBus.Subscribe(this);
        }

        private void AddPoints(PlayerRef playerRef)
        {
            if (_gameplaySessionDataRepository.ContainsKey(playerRef))
                _gameplaySessionDataRepository.GetData(playerRef).ChangePoints(1);
        }

        public void OnEvent(OnKill t)
        {
            if (_entitiesRepository.TryGetPlayer(t.Killer, out var player)) 
                AddPoints(player.PlayerRef);
        }
    }
}