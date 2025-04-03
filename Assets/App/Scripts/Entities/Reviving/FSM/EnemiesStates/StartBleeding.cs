using App.Health;

namespace App.Entities.Reviving.FSM.EnemiesStates
{
    public class StartBleeding : EnemyReviveState
    {
        private readonly ReviveConfig _config;

        public StartBleeding(NetReviver netReviver, NetHealth netHealth, ReviveConfig config) 
            : base(netReviver, netHealth)
        {
            _config = config;
        }

        protected override void OnEnterState() 
            => NetReviver.BleedTimer = _config.BleedingTime;

        protected override void OnFixedUpdate() 
            => TryActivateState<Bleeding>();
    }
}