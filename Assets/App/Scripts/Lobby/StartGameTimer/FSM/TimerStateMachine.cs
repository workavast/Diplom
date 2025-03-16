using System;
using System.Collections.Generic;
using App.FSM;

namespace App.Lobby.StartGameTimer.FSM
{
    public class TimerStateMachine : StateMachine<Type>
    {
        public TimerStateMachine(IReadOnlyCollection<TimerState> states, Type initialState)
            : base(states, initialState) { }
    }
}