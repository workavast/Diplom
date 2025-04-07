using App.Entities;
using UnityEngine;

namespace App.Ai.FSM
{
    public class WaitState : AiState
    {
        private float _startWaitTime;
        private float _targetWaitTime;

        public WaitState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override void OnEnterState()
        {
            _startWaitTime = Runner.SimulationTime;
            _targetWaitTime = Random.Range(Config.WaitMinDuration, Config.WaitMaxDuration);
        }

        protected override void OnFixedUpdate()
        {
            Stay();

            if (AiViewZone.IsSeeAnyPlayer())
            {
                TryActivateState<CombatState>();
                return;
            }
            
            if (Runner.SimulationTime - _startWaitTime >= _targetWaitTime)
            {
                TryActivateState<ChaseState>();
                return;
            }
        }

        private void Stay() 
            => NetEntity.CalculateVelocity(0, 0, false);
    }
}