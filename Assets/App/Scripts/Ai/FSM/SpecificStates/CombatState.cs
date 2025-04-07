using App.Entities;
using UnityEngine;

namespace App.Ai.FSM
{
    public class CombatState : AiState
    {
        private float _startStaySimulationTime;
        private float _targetChangePositionTime;
        private Vector3 _targetCombatPosition;

        public CombatState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override bool CanEnterState() => AiViewZone.IsSeeAnyPlayer();

        protected override void OnEnterState()
        {
            Target = AiViewZone.GetNearestVisiblePlayer();
            _targetCombatPosition = NetEntity.transform.position;
            SetChangePositionInterval();
        }

        protected override void OnExitState()
        {
            NetEntity.TryReload();
        }
        
        protected override void OnFixedUpdate()
        {
            if (Target == null)
            {
                TryActivateState<Idle>();
                return;
            }

            if (LostTarget())
            {
                TryActivateState<WaitState>();
                return;
            }

            LookAtTarget();

            CheckChangePositionTimer();
            if (!ArriveTargetPosition()) 
                MoveToTargetPosition();
            else
            {
                _targetCombatPosition = NetEntity.transform.position;
                NetEntity.CalculateVelocity(0, 0, false);
            }

            NetEntity.TryShoot();
        }

        private bool LostTarget()
        {
            if(!AiViewZone.EntityIsVisible(Target))
            {
                var newTarget = AiViewZone.GetNearestVisiblePlayer();
                if (newTarget == null)
                    return true;

                Target = newTarget;
            }

            return false;
        }

        private void LookAtTarget()
        {
            var lookDirection = (Target.Transform.position - NetEntity.transform.position).normalized;
            NetEntity.RotateByLookDirection(new Vector2(lookDirection.x, lookDirection.z));
        }
        
        private void MoveToTargetPosition()
        {
            var direction = (_targetCombatPosition - NetEntity.transform.position).normalized;
            NetEntity.CalculateVelocity(direction.x, direction.z, false);
        }

        private void CheckChangePositionTimer()
        {
            if (Runner.SimulationTime - _startStaySimulationTime > _targetChangePositionTime)
            {
                var moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

                var changeDistance = Random.Range(Config.PositionChangeMinDistance, Config.PositionChangeMaxDistance);
                _targetCombatPosition += moveDirection * changeDistance;
                SetChangePositionInterval();
            }
        }

        private bool ArriveTargetPosition() 
            => Vector3.Distance(NetEntity.transform.position, _targetCombatPosition) <= Config.ChangePositionTolerance;

        private void SetChangePositionInterval()
        {
            _startStaySimulationTime = Runner.SimulationTime;
            _targetChangePositionTime = Random.Range(Config.PositionChangeIntervalMin, Config.PositionChangeIntervalMax);
        }
    }
}