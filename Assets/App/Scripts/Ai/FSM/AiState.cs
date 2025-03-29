using App.Entities;
using Fusion;
using Fusion.Addons.FSM;

namespace App.Ai.FSM
{
    public abstract class AiState : State
    {
        protected readonly NetAi NetAi;
        protected readonly NetEntity NetEntity;

        protected PlayerRef Owner => NetAi.Object.InputAuthority;
        protected AiConfig AiConfig => NetAi.AiConfig;
        protected NetworkRunner Runner => NetAi.Runner;
        
        protected AiState(NetAi netAi, NetEntity netEntity)
        {
            NetAi = netAi;
            NetEntity = netEntity;
        }
    }
}