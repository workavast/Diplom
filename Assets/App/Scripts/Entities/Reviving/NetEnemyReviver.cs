using System.Collections.Generic;
using App.Entities.Reviving.FSM;
using App.Entities.Reviving.FSM.EnemiesStates;
using App.Health;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Entities.Reviving
{
    public class NetEnemyReviver : NetReviver, IStateMachineOwner
    {
        [SerializeField] private ReviveConfig reviveConfig;
        [SerializeField] private NetHealth netHealth;
        
        private ReviveStateMachine _fsm;
        private None _none;
        private StartBleeding _startBleeding;
        private Bleeding _bleeding;

        public override void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _none = new None(this, netHealth);
            _startBleeding = new StartBleeding(this, netHealth, reviveConfig);
            _bleeding = new Bleeding(this, netHealth);
            
            _fsm = new ReviveStateMachine("Revive - Enemy", _none, _startBleeding, _bleeding);
            
            stateMachines.Add(_fsm);
        }
    }
}