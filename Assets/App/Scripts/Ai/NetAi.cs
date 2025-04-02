using System;
using System.Collections.Generic;
using App.Ai.FSM;
using App.Entities;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Ai
{
    [RequireComponent(typeof(StateMachineController))]
    public class NetAi : NetworkBehaviour, IStateMachineOwner
    {
        [field: SerializeField] public AiConfig AiConfig { get; private set; }
        [SerializeField] private NetEntity netEntity;
        [SerializeField] private AiViewZone aiViewZone;

        private readonly Dictionary<Type, AiState> _states = new(4);
        private readonly AiModel _aiModel = new();
        private AiStateMachine _fsm;
        
        private Idle _idle;
        private ChaseState _chase;
        private WaitState _wait;
        private CombatState _combat;
        private Knockout _knockout;
        private Dead _dead;
        
         public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _idle = new Idle(this, netEntity, _aiModel, aiViewZone);
            _chase = new ChaseState(this, netEntity, _aiModel, aiViewZone);
            _wait = new WaitState(this, netEntity, _aiModel, aiViewZone);
            _combat = new CombatState(this, netEntity, _aiModel, aiViewZone);
            _knockout = new Knockout(this, netEntity, _aiModel, aiViewZone);
            _dead = new Dead(this, netEntity, _aiModel, aiViewZone);
            
            _states.Add(_idle.GetType(), _idle);
            _states.Add(_chase.GetType(), _chase);
            _states.Add(_wait.GetType(), _wait);
            _states.Add(_combat.GetType(), _combat);
            _states.Add(_knockout.GetType(), _knockout);
            _states.Add(_dead.GetType(), _dead);
            
            _fsm = new AiStateMachine("Ai", _idle, _chase, _wait, _combat, _knockout, _dead);
            stateMachines.Add(_fsm);
        }

         public override void FixedUpdateNetwork()
         {
             if (!netEntity.IsAlive() && _fsm.ActiveState != null && _fsm.ActiveState != _knockout && _fsm.ActiveState != _dead)
             {
                 _fsm.TryActivateState<Dead>();
             }
         }
    }
}