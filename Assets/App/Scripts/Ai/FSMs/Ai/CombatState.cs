using System.Collections.Generic;
using App.Ai.FSMs.Movement;
using App.Ai.FSMs.Weapon;
using App.Entities;
using Avastrad.Vector2Extension;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Ai.FSMs.Ai
{
    public class CombatState : AiState
    {
        private float _lostTargetSimulationTime;
        private float _lostTargetTime;

        private StateMachine<MovementState> _movementFsm;
        private StateMachine<WeaponState> _weaponFsm;

        public CombatState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override void CollectChildStateMachines(List<IStateMachine> stateMachines)
        {
            var stay = new Stay(NetEntity, Config.StayMinDuration, Config.StayMaxDuration);
            var randomMove = new RandomMove(NetEntity, Config.MoveMinDistance, Config.MoveMaxDistance,
                Config.MoveTolerance);
            _movementFsm = new StateMachine<MovementState>("Ai-Movement", stay, randomMove);
            stateMachines.Add(_movementFsm);

            var pause = new Pause(NetEntity, Config.PauseMinDuration, Config.PauseMaxDuration, AiViewZone, AiModel);
            var reload = new Reload(NetEntity);
            var shooting = new Shooting(NetEntity, Config.ShotsMinCount, Config.ShotsMaxCount);
            _weaponFsm = new StateMachine<WeaponState>("Ai-Weapon", pause, reload, shooting);
            stateMachines.Add(_weaponFsm);
        }

        protected override bool CanEnterState() => AiViewZone.IsSeeAnyPlayer();

        protected override void OnEnterState()
        {
            Target = AiViewZone.GetNearestVisiblePlayer();
            _movementFsm.TryActivateState<Stay>();
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
                if (_lostTargetSimulationTime <= 0)
                {
                    _lostTargetSimulationTime = Runner.SimulationTime;
                    _lostTargetTime = Random.Range(Config.WaitMinDuration, Config.WaitMaxDuration);
                }

                if (Runner.SimulationTime - _lostTargetSimulationTime >= _lostTargetTime)
                {
                    TryActivateState<HoldPositionState>();
                    return;
                }
            }
            else
            {
                _lostTargetSimulationTime = 0;
                _lostTargetTime = 0;
                
                AiModel.LastTargetPosition = Target.Transform.position;
            }

            LookAt(AiModel.LastTargetPosition);
        }

        private bool LostTarget()
        {
            if (!AiViewZone.EntityIsVisible(Target))
            {
                var newTarget = AiViewZone.GetNearestVisiblePlayer();
                if (newTarget == null)
                    return true;

                Target = newTarget;
            }

            return false;
        }

        private void LookAt(Vector3 point)
        {
            var lookDirection = (point - NetEntity.transform.position).normalized;
            NetEntity.RotateByLookDirection(lookDirection.XZ());
        }
    }
}