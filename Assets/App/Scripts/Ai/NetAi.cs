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
        
        private AiStateMachine _fsm;
        private Idle _idle;
        
        public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _idle = new Idle(this, netEnemy);
            
            _fsm = new AiStateMachine("Ai", _idle);

            stateMachines.Add(_fsm);
        }
    }
}