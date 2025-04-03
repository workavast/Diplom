using System.Collections.Generic;
using App.Entities.Player;
using App.Entities.Reviving.FSM;
using App.Entities.Reviving.FSM.SpecificStates;
using App.Health;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;
using Zenject;

namespace App.Entities.Reviving
{
    public class NetReviver : NetworkBehaviour, IStateMachineOwner
    {
        [Networked] public TickTimer ReviveTimer { get; set; }
        
        [SerializeField] private NetHealth netHealth;
        [SerializeField] private ReviveView reviveView;
        [SerializeField] private ReviveConfig config;

        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository; 
        
        private ReviveStateMachine _fsm;
        private None _none;
        private WaitRevive _waitRevive;
        private ReviveProcess _reviveProcess;
        
        public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _none = new None(this, netHealth, reviveView);
            _waitRevive = new WaitRevive(this, netHealth, reviveView,_playersEntitiesRepository, config);
            _reviveProcess = new ReviveProcess(this, netHealth, reviveView, _playersEntitiesRepository, config);
            
            _fsm = new ReviveStateMachine("Revive", _none, _waitRevive, _reviveProcess);
            stateMachines.Add(_fsm);
        }
    }
}