using App.Entities;
using Avastrad.Vector2Extension;

namespace App.Ai.FSM
{
    public class ChaseState : AiState
    {
        private float _lostTime;

        public ChaseState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override bool CanEnterState()
        {
            return Target != null && !AiViewZone.IsSeeAnyPlayer();
        }

        protected override void OnEnterState()
        {
            _lostTime = Runner.SimulationTime;
        }

        protected override void OnFixedUpdate()
        {
            if (Target == null || Target.Transform == null)
            {
                TryActivateState<Idle>();
                return;
            }

            MoveToTheTarget();

            // Проверка восстановления видимости
            if (AiViewZone.EntityIsVisible(Target))
            {
                TryActivateState<CombatState>();
            }
            else if (Runner.SimulationTime - _lostTime > AiConfig.ChaseDuration)
            {
                TryActivateState<WaitState>();
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