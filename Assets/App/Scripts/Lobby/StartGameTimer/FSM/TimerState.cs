using System;
using App.FSM;
using Fusion;

namespace App.Lobby.StartGameTimer.FSM
{
    public abstract class TimerState : State<Type>
    {
        public override Type Id => GetType();
        
        protected readonly NetStartGameTimerModel NetStartGameTimerModel;
        protected readonly ReadyChecker ReadyChecker;
        protected readonly Action<Type> SetStateDelegate;
        protected NetworkRunner Runner => NetStartGameTimerModel.Runner;
        
        protected NetworkBool IsActive
        {
            get => NetStartGameTimerModel.IsActive;
            set => NetStartGameTimerModel.IsActive = value;
        }
        protected TickTimer NetTimer
        {
            get => NetStartGameTimerModel.NetTimer;
            set => NetStartGameTimerModel.NetTimer = value;
        }
        
        protected TimerState(NetStartGameTimerModel startGameTimerModel, ReadyChecker readyChecker, Action<Type> setStateDelegate)
        {
            NetStartGameTimerModel = startGameTimerModel;
            ReadyChecker = readyChecker;
            SetStateDelegate = setStateDelegate;
        }
        
        public override void OnEnter()
        {
            ReadyChecker.OnDataChanged += UpdateTimerState;
        }

        public override void OnExit()
        {
            ReadyChecker.OnDataChanged -= UpdateTimerState;
        }

        protected abstract void UpdateTimerState();
    }
}