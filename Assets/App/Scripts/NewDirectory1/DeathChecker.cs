using App.Entities.Player;
using App.EventBus;
using Avastrad.EventBusFramework;
using UnityEngine;
using Zenject;

namespace App.NewDirectory1
{
    public class DeathChecker : MonoBehaviour, IEventReceiver<OnPlayerKnockout>, IEventReceiver<OnPlayerDeath>
    {
        [SerializeField] private NetGameState netGameState;
        
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        [Inject] private readonly IEventBus _eventBus;
        
        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();

        private void Awake()
        {
            _eventBus.Subscribe<OnPlayerKnockout>(this);
            _eventBus.Subscribe<OnPlayerDeath>(this);
        }

        public void OnEvent(OnPlayerKnockout e)
        {
            var playerEntities = _playersEntitiesRepository.PlayerEntities;

            foreach (var playerEntity in playerEntities)
                if (playerEntity.IsAlive())
                    return;
            
            netGameState.SetGameState(false);
        }
        
        public void OnEvent(OnPlayerDeath e)
        {
            var playerEntities = _playersEntitiesRepository.PlayerEntities;

            foreach (var playerEntity in playerEntities)
                if (playerEntity.IsAlive())
                    return;
            
            netGameState.SetGameState(false);
        }
    }
}