using App.Entities;
using Fusion;

namespace App.Health.FSM.SpecificStates
{
    public class Knockout : HealthState
    {
        private readonly EntityConfig _config;
        
        public Knockout(NetHealth netEntity, EntityConfig config)
            : base(netEntity)
        {
            _config = config;
        }

        protected override void OnEnterState()
        {
            NetHealth.SetHealth(_config.KnockoutHealthPoints);
            NetHealth.NetKnockout = TickTimer.CreateFromSeconds(Runner, _config.KnockoutTime);
        }

        protected override void OnExitState()
        {
            NetHealth.NetKnockout = TickTimer.None;
        }

        protected override void OnFixedUpdate()
        {
            if (NetHealth.NetKnockout.Expired(Runner)) 
                TryActivateState<Dead>();
        }
    }
}