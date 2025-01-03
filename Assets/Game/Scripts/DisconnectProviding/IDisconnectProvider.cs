using System;

namespace BlackRed.Game.DisconnectProviding
{
    public interface IDisconnectProvider
    {
        public event Action OnDisconnectRequest;
    }
}