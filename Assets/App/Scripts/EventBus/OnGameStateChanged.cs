using Avastrad.EventBusFramework;

namespace App.EventBus
{
    public struct OnGameStateChanged : IEvent
    {
        public readonly bool GameIsRunning;

        public OnGameStateChanged(bool gameIsRunning)
        {
            GameIsRunning = gameIsRunning;
        }
    }
}