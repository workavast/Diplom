using App.Entities;
using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.Ai.FSMs.Ai
{
    public class ChaseState : AiState
    {
        private float _startChaseSimulationTime;
        private float _targetChaseDuration;

        public ChaseState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override bool CanEnterState()
        {
            return Target != null && !AiViewZone.IsSeeAnyPlayer();
        }

        protected override void OnEnterState()
        {
            _startChaseSimulationTime = Runner.SimulationTime;
            _targetChaseDuration = Random.Range(Config.ChaseMinDuration, Config.ChaseMaxDuration);
        }

        protected override void OnFixedUpdate()
        {
            if (Target == null || Target.Transform == null)
            {
                TryActivateState<Idle>();
                return;
            }

            MoveToTheTarget();

            if (AiViewZone.EntityIsVisible(Target))
            {
                TryActivateState<CombatState>();
                return;
            }
            else
            {
                if (Runner.SimulationTime - _startChaseSimulationTime > _targetChaseDuration)
                {
                    TryActivateState<Idle>();
                    return;
                }
            }
        }

        private void MoveToTheTarget()
        {
            var direction = (Target.Transform.position - NetEntity.transform.position).normalized;
            NetEntity.CalculateVelocity(direction.x, direction.z, true);
            NetEntity.RotateByLookDirection(direction.XZ());
        }
    }
}