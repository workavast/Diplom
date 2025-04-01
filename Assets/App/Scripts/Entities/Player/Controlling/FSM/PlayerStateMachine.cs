using Fusion.Addons.FSM;

namespace App.Entities.Player.Controlling.FSM
{
    public class PlayerStateMachine : StateMachine<PlayerState>
    {
        public PlayerStateMachine(string name, params PlayerState[] states)
            : base(name, states) { }
    }
}