using System;
using App.Entities.Player;
using App.Health;
using Fusion;

namespace App.Entities.Reviving.FSM.PlayerStates
{
    [Serializable]
    public class ReviveProcess : PlayerReviveState
    {
        private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        private readonly ReviveConfig _config;

        public ReviveProcess(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView,
            PlayersEntitiesRepository playersEntitiesRepository, ReviveConfig config) 
            : base(netReviver, netHealth, reviveView)
        {
            _playersEntitiesRepository = playersEntitiesRepository;
            _config = config;
        }

        protected override void OnEnterState()
        {
            NetReviver.ReviveTimer = TickTimer.CreateFromSeconds(Runner, _config.ReviveTime);
        }

        protected override void OnExitState()
        {
            NetReviver.ReviveTimer = TickTimer.None;
        }
        
        protected override void OnEnterStateRender() 
            => ReviveView.ToggleVisibility(true);
        
        protected override void OnExitStateRender() 
            => ReviveView.ToggleVisibility(false);

        protected override void OnFixedUpdate()
        {
            if (NetReviver.ReviveTimer.Expired(Runner))
            {
                NetReviver.ReviveTimer = TickTimer.None;
                NetHealth.Revive();
                return;
            }
            
            if (NetHealth.IsDead || NetHealth.IsAlive)
            {
                TryActivateState<None>();
                return;
            }

            if (!_playersEntitiesRepository.TryGetNearestPlayer(NetReviver.transform.position, _config.ReviveDistance, out _))
            {
                TryActivateState<Bleeding>();
                return;
            }
        }
        
        protected override void OnRender()
        {
            if (NetReviver.ReviveTimer.IsRunning)
            {
                var remainingTime = NetReviver.ReviveTimer.RemainingTime(Runner).Value;
                ReviveView.SetValue(1 - remainingTime / _config.ReviveTime);
            }
        }
    }
}