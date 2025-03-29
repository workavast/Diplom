using System.Collections.Generic;
using App.Ai.FSM;
using App.Entities;
using App.Entities.Player;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;
using Zenject;

namespace App.Ai
{
    [RequireComponent(typeof(StateMachineController))]
    public class NetAi : NetworkBehaviour, IStateMachineOwner
    {
        [field: SerializeField] public AiConfig AiConfig { get; private set; }
        [SerializeField] private NetEntity netEnemy;
        [SerializeField] private AiViewZone aiViewZone;

        private AiStateMachine _fsm;
        private Idle _idle;
        
         public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _idle = new Idle(this, netEnemy, aiViewZone);
            
            _fsm = new AiStateMachine("Ai", _idle);

            stateMachines.Add(_fsm);
        }
    }
}