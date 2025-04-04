using Fusion;

namespace App.NetworkRunning.Shutdowners.LocalShutdowners
{
    public abstract class LocalShutdowner
    {
        public abstract void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason);
    }
}