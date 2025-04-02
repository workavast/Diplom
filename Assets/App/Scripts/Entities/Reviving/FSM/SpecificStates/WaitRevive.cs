using App.Entities.Player;
using App.Health;

namespace App.Entities.Reviving.FSM.SpecificStates
{
    public class WaitRevive : ReviveState
    {
        private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        private readonly ReviveConfig _config;

        public WaitRevive(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView,
            PlayersEntitiesRepository playersEntitiesRepository, ReviveConfig config) 
            : base(netReviver, netHealth, reviveView)
        {
            _playersEntitiesRepository = playersEntitiesRepository;
            _config = config;
        }
        
        protected override void OnFixedUpdate()
        {
            if (NetHealth.IsDead || NetHealth.IsAlive)
            {
                TryActivateState<None>();
                return;
            }
            
            if (_playersEntitiesRepository.TryGetNearestPlayer(NetReviver.transform.position, _config.ReviveDistance, out _))
                TryActivateState<ReviveProcess>();
        }

        protected override void OnEnterStateRender() 
            => ReviveView.ToggleVisibility(false);
    }
}