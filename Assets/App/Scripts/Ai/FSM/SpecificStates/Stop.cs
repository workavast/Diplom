using App.Entities;

namespace App.Ai.FSM
{
    public class Stop : AiState
    {
        public Stop(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }
    }
}