using System.Collections.Generic;
using Fusion;
using Fusion.Addons.FSM;

namespace App.Entities.Reviving
{
    public abstract class NetReviver : NetworkBehaviour, IStateMachineOwner
    {
        [Networked] public TickTimer ReviveTimer { get; set; }
        [Networked] [field: ReadOnly] public float BleedTimer { get; set; }

        public abstract void CollectStateMachines(List<IStateMachine> stateMachines);
    }
}