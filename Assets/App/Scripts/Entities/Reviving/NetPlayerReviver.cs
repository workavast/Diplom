using System.Collections.Generic;
using App.Entities.Player;
using App.Entities.Reviving.FSM;
using App.Entities.Reviving.FSM.PlayerStates;
using App.Health;
using Fusion.Addons.FSM;
using UnityEngine;
using Zenject;

namespace App.Entities.Reviving
{
    public class NetPlayerReviver : NetReviver, IStateMachineOwner
    {
        [SerializeField] private NetHealth netHealth;
        [SerializeField] private ReviveConfig config;
        [SerializeField] private ReviveView reviveView;
        [SerializeField] private BleedingView bleedingView;
        
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository; 
        
        private ReviveStateMachine _fsm;
        private None _none;
        private StartBleeding _startBleeding;
        private Bleeding _bleeding;
        private ReviveProcess _reviveProcess;

        private void Awake()
        {
            reviveView.ToggleVisibility(false);
            bleedingView.ToggleVisibility(false);
        }

        public override void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _none = new None(this, netHealth, reviveView);
            _startBleeding = new StartBleeding(this, netHealth, reviveView);
            _bleeding = new Bleeding(this, netHealth, reviveView, bleedingView, _playersEntitiesRepository, config);
            _reviveProcess = new ReviveProcess(this, netHealth, reviveView, _playersEntitiesRepository, config);
            
            _fsm = new ReviveStateMachine("Revive - Player", _none, _startBleeding, _bleeding, _reviveProcess);
            
            stateMachines.Add(_fsm);
        }
    }
}