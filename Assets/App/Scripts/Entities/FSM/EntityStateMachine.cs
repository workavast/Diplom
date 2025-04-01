using Fusion.Addons.FSM;

namespace App.Entities.FSM
{
    public class EntityStateMachine : StateMachine<EntityState>
    {
        public EntityStateMachine(string name, params EntityState[] states)
            : base(name, states) { }
    }
}