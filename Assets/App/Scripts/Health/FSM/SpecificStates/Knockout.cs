using App.Entities;

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
        }
    }
}