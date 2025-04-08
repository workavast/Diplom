using App.Ai.Config;
using App.Entities;
using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.Ai.FSMs.Ai
{
    public class ChaseState : AiState
    {
        private ChaseConfig Config => AiConfig.ChaseConfig;
        
        public ChaseState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override bool CanEnterState()
        {
            return Target != null && !AiViewZone.IsSeeAnyPlayer();
        }

        protected override void OnFixedUpdate()
        {
            MoveToThePoint(AiModel.LastHashedTargetPosition);

            if (AiViewZone.EntityIsVisible(Target))
            {
                TryActivateState<CombatState>();
                return;
            }
            else
            {
                if (ArrivePosition(AiModel.LastHashedTargetPosition, AiConfig.MoveTolerance))
                {
                    TryActivateState<Idle>();
                    return;
                }
            }
        }

        private void MoveToThePoint(Vector3 point)
        {
            var direction = (point - NetEntity.transform.position).normalized;
            NetEntity.CalculateVelocity(direction.x, direction.z, true);
            NetEntity.RotateByLookDirection(direction.XZ());
        }

        private bool ArrivePosition(Vector3 targetPosition, float tolerance) 
            => Vector2.Distance(NetEntity.transform.position.XZ(), targetPosition.XZ()) <= tolerance;
    }
}