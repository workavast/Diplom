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
        [SerializeField] private NetEntity netEnemy;
        [SerializeField] private AiViewZone aiViewZone;

        private readonly Dictionary<Type, AiState> _states = new(4);
        private readonly AiModel _aiModel = new();
        private AiStateMachine _fsm;
        
        private Idle _idle;
        private ChaseState _chase;
        private WaitState _wait;
        private CombatState _combat;
        
         public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _idle = new Idle(this, netEnemy, _aiModel, aiViewZone);
            _chase = new ChaseState(this, netEnemy, _aiModel, aiViewZone);
            _wait = new WaitState(this, netEnemy, _aiModel, aiViewZone);
            _combat = new CombatState(this, netEnemy, _aiModel, aiViewZone);
            
            _states.Add(_idle.GetType(), _idle);
            _states.Add(_chase.GetType(), _chase);
            _states.Add(_wait.GetType(), _wait);
            _states.Add(_combat.GetType(), _combat);
            
            _fsm = new AiStateMachine("Ai", _idle, _chase, _wait, _combat);

            stateMachines.Add(_fsm);
        }

         public string ActiveState;
         public void TryActivateState<TState>() 
             where TState : AiState
         {
             Debug.Log("TryActivateState");
             ActiveState = typeof(TState).ToString();
             _fsm.TryActivateState(_states[typeof(TState)]);
         }
    }
}