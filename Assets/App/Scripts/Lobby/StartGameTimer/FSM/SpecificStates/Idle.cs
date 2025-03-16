using System;
using Fusion;

namespace App.Lobby.StartGameTimer.FSM.SpecificStates
{
    public class Idle : TimerState
    {
        public Idle(NetStartGameTimerModel startGameTimerModel, ReadyChecker readyChecker, Action<Type> setStateDelegate) 
            : base(startGameTimerModel, readyChecker, setStateDelegate) { }
        
        public override void OnEnter()
        {
            base.OnEnter();

            IsActive = false;

            if (NetStartGameTimerModel.IsSpawned) 
                NetStartGameTimerModel.NetTimer = TickTimer.None;
        }

        protected override void UpdateTimerState()
        {
            if (Runner == null)
                return;

            if (!ReadyChecker.PlayerIsReady(Runner.LocalPlayer))
                return;

            if (ReadyChecker.ReadyCounter.IsFull)
            {
                SetStateDelegate?.Invoke(typeof(ShortTimer));
                return;
            }

            if (ReadyChecker.ReadyCounter.FillingPercentage >= 0.5f)
            {
                SetStateDelegate?.Invoke(typeof(LongTimer));
                return;
            }
        }
    }
}