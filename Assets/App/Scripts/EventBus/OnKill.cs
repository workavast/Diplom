using Avastrad.EventBusFramework;

namespace App.EventBus
{
    public struct OnKill : IEvent
    {
        public readonly int Killer;
        public readonly int Killed;

        public OnKill(int killed, int killer)
        {
            Killer = killer;
            Killed = killed;
        }
    }
}