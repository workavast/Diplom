using App.Entities;

namespace App.Ai.FSM
{
    public class Dead : AiState
    {
        public Dead(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }
    }
}