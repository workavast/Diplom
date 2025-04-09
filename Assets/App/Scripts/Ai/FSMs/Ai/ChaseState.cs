using App.Ai.Config;
using App.Entities;
using UnityEngine.AI;

namespace App.Ai.FSMs.Ai
{
    public class ChaseState : AiState
    {
        private readonly NavMeshPath _path = new();
        private int _currentCorner;
        
        private ChaseConfig Config => AiConfig.ChaseConfig;
        
        public ChaseState(NetAi netAi, NetEntity netEntity, AiModel aiModel, AiViewZone aiViewZone)
            : base(netAi, netEntity, aiModel, aiViewZone) { }

        protected override bool CanEnterState()
        {
            return Target != null && !AiViewZone.IsSeeAnyPlayer();
        }

        protected override void OnEnterState()
        {
            NetEntity.GetPath(AiModel.LastHashedTargetPosition, _path);
            _currentCorner = 0;
        }

        protected override void OnEnterStateRender()
        {
            NetEntity.DrawPath(_path);
        }

        protected override void OnFixedUpdate()
        {
            if (AiViewZone.EntityIsVisible(Target))
            {
                TryActivateState<CombatState>();
                return;
            }
            
            if (NetEntity.MoveToPoint(_path, AiConfig.MoveTolerance, ref _currentCorner))
            {
                TryActivateState<Idle>();
                return;
            }
            
            NetEntity.LookAtPoint(AiModel.LastHashedTargetPosition);
        }
    }
}