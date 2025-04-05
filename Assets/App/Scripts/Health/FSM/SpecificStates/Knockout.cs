using System;
using App.Entities;

namespace App.Health.FSM.SpecificStates
{
    public class Knockout : HealthState
    {
        private readonly EntityConfig _config;

        public event Action OnActivate;
        
        public Knockout(NetHealth netEntity, EntityConfig config)
            : base(netEntity)
        {
            _config = config;
        }

        protected override void OnEnterState()
        {
            NetHealth.SetHealth(_config.KnockoutHealthPoints);
            
            OnActivate?.Invoke();
        }
    }
}