using App.Entities;
using Avastrad.Vector2Extension;

namespace App.Ai.FSMs.Ai
{
    public class Idle : AiState
    {
        public Idle(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override void OnFixedUpdate()
        {
            NetEntity.RotateByLookDirection(NetEntity.transform.forward.XZ());
            NetEntity.CalculateVelocity(0, 0, false);

            if (AiViewZone.IsSeeAnyPlayer())
            {
                TryActivateState<CombatState>();
                return;
            }

            if (NetEntity.CanReload) 
                NetEntity.TryReload();
        }
    }
}