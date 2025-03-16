using System;

namespace App.Lobby.StartGameTimer.FSM.SpecificStates
{
    public class IsOver : TimerState
    {
        public IsOver(NetStartGameTimerModel startGameTimerModel, ReadyChecker readyChecker, 
            Action<Type> setStateDelegate) 
            : base(startGameTimerModel, readyChecker, setStateDelegate)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            IsActive = false;
        }

        protected override void UpdateTimerState()
        {
            
        }
    }
}