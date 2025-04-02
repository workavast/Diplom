using App.Entities;
using UnityEngine;

namespace App.Ai.FSM
{
    public class CombatState : AiState
    {
        private float _lastPositionChangeTime;
        private Vector3 _currentCombatPosition;

        public CombatState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override bool CanEnterState() => AiViewZone.IsSeeAnyPlayer();

        protected override void OnEnterState()
        {
            Target = AiViewZone.GetNearestVisiblePlayer();
            _currentCombatPosition = NetEntity.transform.position;
            _lastPositionChangeTime = Runner.SimulationTime;
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
                TryActivateState<WaitState>();

            // if(!AiViewZone.EntityIsVisible(Target))
            // {
            //     var newTarget = AiViewZone.GetNearestVisiblePlayer();
            //     if (newTarget == null)
            //     {
            //         NetAi.TryActivateState<WaitState>();
            //         return;
            //     }
            //
            //     Target = newTarget;
            // }

            LookAtTarget();
            NetEntity.TryShoot();
            HandlePositionChange();
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
        
        private void HandlePositionChange()
        {
            if (Runner.SimulationTime - _lastPositionChangeTime > AiConfig.PositionChangeInterval)
            {
                var moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

                _currentCombatPosition += moveDirection * AiConfig.PositionChangeDistance;
                _lastPositionChangeTime = Runner.SimulationTime;
            }

            var direction = (_currentCombatPosition - NetEntity.transform.position).normalized;
            NetEntity.CalculateVelocity(direction.x, direction.z, false);
        }
    }
}