using System;

namespace App.Lobby.StartGameTimer.FSM.SpecificStates
{
    public class ShortTimer : TimerState
    {
        private readonly float _timeValue;

        public ShortTimer(NetStartGameTimerModel startGameTimerModel, ReadyChecker readyChecker, Action<Type> setStateDelegate, float timeValue) 
            : base(startGameTimerModel, readyChecker, setStateDelegate)
        {
            _timeValue = timeValue;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            IsActive = true;

            if (NetStartGameTimerModel.NetTimer.ExpiredOrNotRunning(Runner))
            {
                NetStartGameTimerModel.SetTime(_timeValue);
            }
            else
            {
                var currentTimeValue = NetStartGameTimerModel.NetTimer.RemainingTime(Runner);
                if (currentTimeValue > _timeValue)
                    NetStartGameTimerModel.SetTime(_timeValue);
            }
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

            if (ReadyChecker.ReadyCounter.FillingPercentage >= 0.5f)
            {
                SetStateDelegate?.Invoke(typeof(LongTimer));
                return;
            }
        }
    }
}