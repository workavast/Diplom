using System;
using System.Collections.Generic;
using Fusion;

namespace App.Players.SessionData
{
    public interface IPlayersSessionDataRepository<T>
        where T : NetworkBehaviour
    {
        public IReadOnlyDictionary<PlayerRef, T> PlayersSessionData { get; }

        public event Action<PlayerRef, T> OnAdd;
        public event Action<PlayerRef, T> OnRemove;
        
        public bool ContainsKey(PlayerRef playerRef);
        public T GetData(PlayerRef playerRef);
    }
}