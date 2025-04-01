namespace App.Entities.FSM.SpecificStates
{
    public class Knockout : EntityState
    {
        private float _startWaitTime;
        
        public Knockout(NetEntity netEntity)
            : base(netEntity) { }

        protected override void OnEnterState()
        {
            _startWaitTime = Runner.SimulationTime;
        }

        protected override void OnFixedUpdate()
        {
            if (Runner.SimulationTime - _startWaitTime > 5) 
                NetEntity.Review(100);

            if (NetEntity.IsAlive()) 
                NetEntity.TryActivateState<Alive>();
        }
    }
}