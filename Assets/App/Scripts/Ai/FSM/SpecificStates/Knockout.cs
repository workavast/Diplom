using App.Entities;

namespace App.Ai.FSM
{
    public class Knockout : AiState
    {
        public Knockout(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }
    }
}