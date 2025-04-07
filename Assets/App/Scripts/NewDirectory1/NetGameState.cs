using App.EventBus;
using Avastrad.EventBusFramework;
using Avastrad.UI.UiSystem;
using Fusion;
using Zenject;

namespace App.NewDirectory1
{
    public class NetGameState : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(OnGameStateChanged))] public bool GameIsRunning { get; private set; } = true;

        [Inject] private readonly ScreensController _screensController;
        [Inject] private readonly IEventBus _eventBus; 
        
        public void SetGameState(bool gameIsRunning)
        {
            if (HasStateAuthority)
                GameIsRunning = gameIsRunning;
        }
        
        private void OnGameStateChanged()
        {
            if (!GameIsRunning)
                _screensController.SetScreen(ScreenType.EndGame);
            
            _eventBus.Invoke(new OnGameStateChanged(GameIsRunning));
        }
    }
}