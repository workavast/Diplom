namespace App.Entities.Health.FSM.SpecificStates
{
    public class Knockout : HealthState
    {
        private readonly EntityConfig _entityConfig;
        
        public Knockout(NetHealth netEntity, EntityConfig entityConfig)
            : base(netEntity)
        {
            _entityConfig = entityConfig;
        }

        protected override void OnEnterState()
        {
            NetHealth.SetHealth(_entityConfig.KnockoutHealthPoints);
        }
    }
}