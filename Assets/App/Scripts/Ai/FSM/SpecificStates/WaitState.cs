using App.Entities;
using UnityEngine;

namespace App.Ai.FSM
{
    public class WaitState : AiState
    {
        private float _startWaitTime;
        private Vector3 _waitPosition;

        public WaitState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override void OnEnterState()
        {
            _startWaitTime = Runner.SimulationTime;
            _waitPosition = NetEntity.transform.position;
        }

        protected override void OnFixedUpdate()
        {
            Stay();

            // Проверка таймера ожидания
            if (Runner.SimulationTime - _startWaitTime > AiConfig.WaitDuration) 
                TryActivateState<ChaseState>();

            // Если игрок появился - переходим в бой
            if (AiViewZone.IsSeeAnyPlayer()) 
                TryActivateState<CombatState>();
        }

        private void Stay() 
            => NetEntity.CalculateVelocity(0, 0, false);
    }
}