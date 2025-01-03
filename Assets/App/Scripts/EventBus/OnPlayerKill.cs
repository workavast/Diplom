using Avastrad.EventBusFramework;
using Fusion;

namespace App.EventBus
{
    public struct OnPlayerKill : IEvent
    {
        public readonly PlayerRef KilledPlayer;
        public readonly PlayerRef Killer;

        public OnPlayerKill(PlayerRef killedPlayer, PlayerRef killer)
        {
            Killer = killer;
            KilledPlayer = killedPlayer;
        }
    }
}