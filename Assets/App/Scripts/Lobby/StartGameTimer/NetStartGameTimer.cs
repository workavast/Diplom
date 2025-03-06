using System;
using App.Lobby.SessionData;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Lobby
{
    public class NetStartGameTimer : NetworkBehaviour
    {
        [SerializeField, Min(0)] private float nonFullTeamStartTime = 90;
        [SerializeField, Min(0)] private float fullTeamStartTime = 10;
        
        [Networked, OnChangedRender(nameof(ActivityChanged))] private NetworkBool IsActive { get; set; }
        [Networked] private TickTimer NetReloadTimer { get; set; }

        [Inject] private readonly LobbySessionDataRepository _lobbySessionDataRepository;

        private bool _spawned;
        private int _lastRemainingTime;
        private TimeSpan _lastTimeSPan;
        private ReadyChecker _readyChecker;

        public event Action<bool> OnActivityChanged;

        [ContextMenu("StartsTimer")]
        public void StartsTimer()
        {
            NetReloadTimer = TickTimer.CreateFromSeconds(Runner, nonFullTeamStartTime);
        }

        public override void Spawned()
        {
            _spawned = true;

            if (HasStateAuthority)
            {
                _readyChecker = new ReadyChecker(_lobbySessionDataRepository);
                _readyChecker.OnDataChanged += UpdateTimerState;

                UpdateTimerState();
            }
            
            OnActivityChanged?.Invoke(IsActive);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _readyChecker?.Dispose();
        }

        public TimeSpan GetTime()
        {
            if (!_spawned || NetReloadTimer.ExpiredOrNotRunning(Runner))
                return TimeSpan.Zero;

            var remainingTime = (int)Math.Floor(NetReloadTimer.RemainingTime(Runner).Value);
            if (_lastRemainingTime == remainingTime)
                return _lastTimeSPan;
            
            _lastRemainingTime = remainingTime;
            
            var minutes = (int)Math.Floor((float)_lastRemainingTime / 60);
            var seconds = (_lastRemainingTime % 60);
            _lastTimeSPan = new TimeSpan(0,0, minutes, seconds);
            
            return _lastTimeSPan;
        }
        
        private void UpdateTimerState()
        {
            if (!_readyChecker.PlayerIsReady(Runner.LocalPlayer))
            {
                IsActive = false;
                return;
            }

            if (_readyChecker.ReadyCounter.IsFull)
            {
                IsActive = true;
                NetReloadTimer = TickTimer.CreateFromSeconds(Runner, fullTeamStartTime);
                return;
            }

            if (_readyChecker.ReadyCounter.FillingPercentage >= 0.5f)
            {
                IsActive = true;
                NetReloadTimer = TickTimer.CreateFromSeconds(Runner, nonFullTeamStartTime);
                return;
            }
        }

        private void ActivityChanged()
        {
            OnActivityChanged?.Invoke(IsActive);
        }
    }
}