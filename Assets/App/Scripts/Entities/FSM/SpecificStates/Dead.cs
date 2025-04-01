using App.Entities.Player.Controlling;

namespace App.Entities.FSM.SpecificStates
{
    public class Dead : EntityState
    {
        private float _startWaitTime;
        
        public Dead(NetEntity netEntity)
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