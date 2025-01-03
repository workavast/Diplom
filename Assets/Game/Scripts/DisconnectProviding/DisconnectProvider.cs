using System;

namespace BlackRed.Game.DisconnectProviding
{
    public class DisconnectProvider : IDisconnectInvoker, IDisconnectProvider
    {
        public event Action OnDisconnectRequest;

        public void Disconnect() 
            => OnDisconnectRequest?.Invoke();
    }
}