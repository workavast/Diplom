using System;
using System.Collections.Generic;
using App.Entities.Health.FSM;
using App.Entities.Health.FSM.SpecificStates;
using App.EventBus;
using Avastrad.EventBusFramework;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;
using Zenject;

namespace App.Entities.Health
{
    public class NetHealth : NetworkBehaviour, IStateMachineOwner
    {
        [SerializeField] protected EntityConfig config;
        [SerializeField] private NetEntity entity;
        
        [Networked] [field: ReadOnly] public float NetHealthPoints { get; private set; }

        [Inject] private IEventBus EventBus { get; set; }

        public bool IsAlive => _fsm.ActiveState == _alive;
        public bool IsKnockout => _fsm.ActiveState == _knockout;
        public bool IsDead => _fsm.ActiveState == _dead;
        
        private readonly Dictionary<Type, HealthState> _states = new(3);
        private HealthStateMachine _fsm;
        private Alive _alive;
        private Knockout _knockout;
        private Dead _dead;
        
        public event Action OnDeath;
        public event Action<IEntity> OnDeathEntity;
        
        public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _alive = new Alive(this);
            _knockout = new Knockout(this, config);
            _dead = new Dead(this);
            
            _states.Add(_alive.GetType(), _alive);
            _states.Add(_knockout.GetType(), _knockout);
            _states.Add(_dead.GetType(), _dead);
            
            _fsm = new HealthStateMachine("Entity", _alive, _knockout, _dead);
            stateMachines.Add(_fsm);
        }

        public override void Spawned()
        {
            NetHealthPoints = config.InitialHealthPoints;
        }

        public void SetHealth(int value)
        {
            NetHealthPoints = value;
        }
        
        public void TakeDamage(float damage, IEntity killer)
        {
            if (_fsm.ActiveState != _alive)
            {
                Debug.LogError($"You try damage entity that un alive: [{gameObject.name}] [{_fsm.ActiveState.GetType()}]");
                return;
            }
            
            NetHealthPoints -= damage;

            if (HasStateAuthority && NetHealthPoints <= 0)
            {
                EventBus.Invoke(new OnKill(entity.Identifier.Id, killer.Identifier.Id));
                OnDeath?.Invoke();
                OnDeathEntity?.Invoke(entity);
                Debug.Log($"{entity.GetName()} is dead");

                if (entity.EntityType == EntityType.Default)
                    Runner.Despawn(Object);
            }
        }

        public void PermanentDeath() 
            => TryActivateState<Dead>();
        
        public void Revive()
        {
            TryActivateState<Alive>();
            SetHealth(config.ReviveHealthPoints);
        }

        /// <summary> Used to debug </summary>
        public string ActiveState;
        public void TryActivateState<TState>() 
            where TState : HealthState
        {
            Debug.Log("TryActivateState");
            ActiveState = typeof(TState).ToString();
            _fsm.TryActivateState(_states[typeof(TState)]);
        }
    }
}