using System.Collections.Generic;
using App.Ai.FSM;
using App.Entities;
using App.EventBus;
using Avastrad.EventBusFramework;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Ai
{
    [RequireComponent(typeof(StateMachineController))]
    public class NetAi : NetworkBehaviour, IStateMachineOwner, IEventReceiver<OnGameStateChanged>
    {
        [field: SerializeField] public AiConfig AiConfig { get; private set; }
        [SerializeField] private NetEntity netEntity;
        [SerializeField] private AiViewZone aiViewZone;
     
        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; } = new();
        
        private readonly AiModel _aiModel = new();
        
        private AiStateMachine _fsm;
        private Idle _idle;
        private ChaseState _chase;
        private WaitState _wait;
        private CombatState _combat;
        private Stop _stop;
        
         public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _idle = new Idle(this, netEntity, _aiModel, aiViewZone);
            _chase = new ChaseState(this, netEntity, _aiModel, aiViewZone);
            _wait = new WaitState(this, netEntity, _aiModel, aiViewZone);
            _combat = new CombatState(this, netEntity, _aiModel, aiViewZone);
            _stop = new Stop(this, netEntity, _aiModel, aiViewZone);
            
            _fsm = new AiStateMachine("Ai", _idle, _chase, _wait, _combat, _stop);
            stateMachines.Add(_fsm);
        }

         public override void FixedUpdateNetwork()
         {
             if (!netEntity.IsAlive() && _fsm.ActiveState != null && _fsm.ActiveState != _stop)
             {
                 _fsm.TryActivateState<Stop>();
             }
         }

         public void OnEvent(OnGameStateChanged e)
         {
             if (!e.GameIsRunning && _fsm.ActiveState != null && _fsm.ActiveState != _stop) 
                 _fsm.TryActivateState<Stop>();
         }
    }
}