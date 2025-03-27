using App.Entities;
using Fusion.Addons.FSM;

namespace App.Ai.FSM
{
    public abstract class AiState : State
    {
        protected readonly NetAi NetAi;
        protected readonly NetEntity NetEntity;

        protected AiState(NetAi netAi, NetEntity netEntity)
        {
            NetAi = netAi;
            NetEntity = netEntity;
        }
    }
}