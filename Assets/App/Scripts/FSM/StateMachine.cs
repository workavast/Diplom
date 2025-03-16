using System;
using System.Collections.Generic;

namespace App.FSM
{
    public class StateMachine<T> : IDisposable
    {
        private readonly Dictionary<T, State<T>> _states;
        private State<T> _activeState;
        
        public StateMachine(IReadOnlyCollection<State<T>> states, T initialState)
        {
            _states = new Dictionary<T, State<T>>(states.Count);
            foreach (var state in states) 
                _states.Add(state.Id, state);
            
            SetState(initialState);
        }

        public void SetState(T stateId)
        {
            _activeState?.OnExit();
            _activeState = _states[stateId];
            _activeState.OnEnter();
        }

        public void OnUpdate() 
            => _activeState?.OnUpdate();

        public void OnFixedUpdate() 
            => _activeState?.OnFixedUpdate();

        public void OnLateUpdate() 
            => _activeState.OnLateUpdate();
        
        public void Dispose() 
            => _activeState?.OnExit();
    }
}