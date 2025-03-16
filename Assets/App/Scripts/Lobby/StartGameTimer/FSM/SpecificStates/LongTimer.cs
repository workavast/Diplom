using System;

namespace App.Lobby.StartGameTimer.FSM.SpecificStates
{
    public class LongTimer : TimerState
    {
        private readonly float _timeValue;

        public LongTimer(NetStartGameTimerModel startGameTimerModel, ReadyChecker readyChecker, Action<Type> setStateDelegate, float timeValue) 
            : base(startGameTimerModel, readyChecker, setStateDelegate)
        {
            _timeValue = timeValue;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            IsActive = true;
            NetStartGameTimerModel.SetTime(_timeValue);
        }

        protected override void UpdateTimerState()
        {
            if (Runner == null)
                return;
            
            if (!ReadyChecker.PlayerIsReady(Runner.LocalPlayer))
            {
                SetStateDelegate?.Invoke(typeof(Idle));
                return;
            }

            if (ReadyChecker.ReadyCounter.IsFull)
            {
                SetStateDelegate?.Invoke(typeof(ShortTimer));
                return;
            }
        }
    }
}