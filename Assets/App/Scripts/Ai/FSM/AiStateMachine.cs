using Fusion.Addons.FSM;

namespace App.Ai.FSM
{
    public class AiStateMachine : StateMachine<AiState>
    {
        public AiStateMachine(string name, params AiState[] states)
            : base(name, states) { }
    }
}