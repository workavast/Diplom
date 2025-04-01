namespace App.Entities.Player.Controlling.FSM.SpecificStates
{
    public class Dead : PlayerState
    {
        private float _startWaitTime;
        
        public Dead(NetPlayerController netAi, NetEntity netEntity)
            : base(netAi, netEntity) { }

        protected override void OnEnterState()
        {
            _startWaitTime = Runner.SimulationTime;
        }

        protected override void OnFixedUpdate()
        {
            if (Runner.SimulationTime - _startWaitTime > 5) 
                NetEntity.Review(100);

            if (NetEntity.IsAlive()) 
                NetController.TryActivateState<Alive>();
        }
    }
}