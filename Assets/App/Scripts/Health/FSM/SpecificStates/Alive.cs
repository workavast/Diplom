using Fusion.Addons.FSM;

namespace App.Health.FSM.SpecificStates
{
    public class Alive : HealthState
    {
        public Alive(NetHealth netHealth) 
            : base(netHealth) { }

        protected override void OnFixedUpdate()
        {
            if (HealthPoints <= 0) 
                Machine.TryActivateState<Knockout>();
        }
    }
}