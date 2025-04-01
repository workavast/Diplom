using System;
using System.Collections.Generic;
using App.Entities.Player.Controlling.FSM;
using App.Entities.Player.Controlling.FSM.SpecificStates;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Entities.Player.Controlling
{
    public class NetPlayerController : NetworkBehaviour, IStateMachineOwner
    {
        [SerializeField] private NetEntity playerEntity;

        private Alive _alive;
        private Dead _dead;

        private readonly Dictionary<Type, PlayerState> _states = new(4);
        private PlayerStateMachine _fsm;
        
        public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _alive = new Alive(this, playerEntity);
            _dead = new Dead(this, playerEntity);
            
            _states.Add(_alive.GetType(), _alive);
            _states.Add(_dead.GetType(), _dead);
            
            _fsm = new PlayerStateMachine("PlayerController", _alive, _dead);
            stateMachines.Add(_fsm);
        }
        
        public void TryActivateState<TState>() where TState : PlayerState =>
            _fsm.TryActivateState(_states[typeof(TState)]);
    }
}