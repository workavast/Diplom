using App.NetworkRunning.Shutdowners.LocalShutdowners;
using Fusion;

namespace App.NetworkRunning.Shutdowners
{
    public class ShutdownerProvider
    {
        private LocalShutdowner _localShutdowner;

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) 
            => _localShutdowner.OnShutdown(runner, shutdownReason);
        
        public void SetLocalShutdownProvider(LocalShutdowner localShutdowner) 
            => _localShutdowner = localShutdowner;
    }
}