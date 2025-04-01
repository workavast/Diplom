using Fusion.Addons.FSM;

namespace App.Health.FSM
{
    public class HealthStateMachine : StateMachine<HealthState>
    {
        public HealthStateMachine(string name, params HealthState[] states)
            : base(name, states) { }
    }
}