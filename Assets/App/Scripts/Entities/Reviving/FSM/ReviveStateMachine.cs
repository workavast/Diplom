using Fusion.Addons.FSM;

namespace App.Entities.Reviving.FSM
{
    public class ReviveStateMachine : StateMachine<ReviveState>
    {
        public ReviveStateMachine(string name, params ReviveState[] states)
            : base(name, states) { }
    }
}