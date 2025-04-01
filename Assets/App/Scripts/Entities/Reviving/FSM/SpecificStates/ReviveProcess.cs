using App.Entities.Player;
using App.Health;
using Fusion;

namespace App.Entities.Reviving.FSM.SpecificStates
{
    public class ReviveProcess : ReviveState
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

        protected override void OnFixedUpdate()
        {
            if (NetHealth.IsDead || NetHealth.IsAlive)
            {
                NetReviver.TryActivateState<None>();
                return;
            }

            if (NetReviver.ReviveTimer.Expired(Runner))
            {
                NetHealth.Revive();
                return;
            }

            if (!_playersEntitiesRepository.TryGetNearestPlayer(NetReviver.transform.position, _config.ReviveDistance, out _))
            {
                NetReviver.TryActivateState<WaitRevive>();
                return;
            }
        }

        protected override void OnRender()
        {
            ReviveView.ToggleVisibility(true);

            if (NetReviver.ReviveTimer.IsRunning)
            {
                var remainingTime = NetReviver.ReviveTimer.RemainingTime(Runner).Value;
                ReviveView.SetValue(1 - remainingTime / _config.ReviveTime);
            }
        }
    }
}