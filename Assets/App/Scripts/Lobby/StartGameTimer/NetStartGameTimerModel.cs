using System;
using Fusion;

namespace App.Lobby.StartGameTimer
{
    public class NetStartGameTimerModel : NetworkBehaviour, IReadOnlyGameStartTimer
    {
        [Networked, OnChangedRender(nameof(ActivityChanged))] private NetworkBool isActive { get; set; }
        [Networked] [field: ReadOnly] public TickTimer NetTimer { get; set; }

        private bool _isActiveHashed;
        public NetworkBool IsActive
        {
            get
            {
                if (IsSpawned)
                    return isActive;

                return _isActiveHashed;
            }
            set
            {
                if (IsSpawned)
                    isActive = value;

                _isActiveHashed = value;
            }
        }
        public bool IsOver { get; private set; }
        public bool IsSpawned { get; private set; }

        private int _lastRemainingTime;
        private TimeSpan _lastTimeSPan;
        
        public event Action OnTimerIsOver;
        public event Action<bool> OnActivityChanged;
        
        public override void Spawned()
        {
            IsSpawned = true;

            if (HasStateAuthority)
                isActive = _isActiveHashed;
            
            OnActivityChanged?.Invoke(isActive);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            IsSpawned = false;
        }

        public override void FixedUpdateNetwork()
        {
            if (!IsOver && NetTimer.Expired(Runner))
            {
                IsOver = true;
                OnTimerIsOver?.Invoke();
            }
        }
        
        public void SetTime(float time)
        {
            NetTimer = TickTimer.CreateFromSeconds(Runner, time);
            IsOver = false;
        }
        
        public TimeSpan GetTimeSpan()
        {
            if (!IsSpawned || NetTimer.ExpiredOrNotRunning(Runner))
                return TimeSpan.Zero;

            var remainingTime = (int)Math.Floor(NetTimer.RemainingTime(Runner).Value);
            if (_lastRemainingTime == remainingTime)
                return _lastTimeSPan;
            
            _lastRemainingTime = remainingTime;
            
            var minutes = (int)Math.Floor((float)_lastRemainingTime / 60);
            var seconds = (_lastRemainingTime % 60);
            _lastTimeSPan = new TimeSpan(0,0, minutes, seconds);
            
            return _lastTimeSPan;
        }
        
        private void ActivityChanged() 
            => OnActivityChanged?.Invoke(isActive);
    }
}