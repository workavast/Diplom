using System;

namespace App.DisconnectProviding
{
    public interface IDisconnectProvider
    {
        public event Action OnDisconnectRequest;
    }
}