using System.Collections.Generic;
using App.Ai.Config;
using App.Ai.FSMs.Movement;
using App.Entities;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Ai.FSMs.Ai
{
    public class HoldPositionState : AiState
    {
        private HoldPositionConfig Config => AiConfig.HoldPositionConfig;
        private float _targetWaitTime;

        private StateMachine<MovementState> _movementFsm;

        public HoldPositionState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override void CollectChildStateMachines(List<IStateMachine> stateMachines)
        {
            var stay = new Stay(NetEntity, Config.StayMinDuration, Config.StayMaxDuration);
            var randomMove = new RandomMove(NetEntity, Config.MoveMinDistance, Config.MoveMaxDistance, AiConfig.MoveTolerance);
            
            _movementFsm = new StateMachine<MovementState>("HoldPosition-Movement", stay, randomMove);
            stateMachines.Add(_movementFsm);
        }

        protected override void OnEnterState()
        {
            _targetWaitTime = Random.Range(Config.HoldPositionMinDuration, Config.HoldPositionMaxDuration);
        }

        protected override void OnFixedUpdate()
        {
            if (AiViewZone.IsSeeAnyPlayer())
            {
                TryActivateState<CombatState>();
                return;
            }
            
            if (StateTime >= _targetWaitTime)
            {
                TryActivateState<ChaseState>();
                return;
            }
        }
    }
}