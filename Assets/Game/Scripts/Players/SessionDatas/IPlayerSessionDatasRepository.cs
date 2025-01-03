using System;
using System.Collections.Generic;
using Fusion;

namespace BlackRed.Game.Players.SessionDatas
{
    public interface IPlayerSessionDatasRepository
    {
        public IReadOnlyDictionary<PlayerRef, NetPlayerSessionData> Datas { get; }

        public event Action<PlayerRef, NetPlayerSessionData> OnPlayerAdd;
        public event Action<PlayerRef> OnPlayerRemove;

        public void Add(PlayerRef playerRef, NetPlayerSessionData playerView);
        public void Remove(PlayerRef playerRef);
    }
}