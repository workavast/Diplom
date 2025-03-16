using System;
using Fusion;

namespace App.Lobby.StartGameTimer
{
    public interface IReadOnlyGameStartTimer
    {
        public NetworkBool IsActive { get; }
        public TickTimer NetTimer { get; }
        
        public event Action OnTimerIsOver;
        public event Action<bool> OnActivityChanged;
        
        public TimeSpan GetTimeSpan();
    }
}