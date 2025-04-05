using Avastrad.UI.UiSystem;
using Fusion;
using Zenject;

namespace App.NewDirectory1
{
    public class NetGameState : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(OnGameStateChanged))] public bool GameIsRunning { get; private set; } = true;

        [Inject] private ScreensController _screensController;

        public void SetGameState(bool gameIsRunning)
        {
            if (HasStateAuthority)
                GameIsRunning = gameIsRunning;
        }
        
        private void OnGameStateChanged()
        {
            if (!GameIsRunning) 
                _screensController.SetScreen(ScreenType.EndGame);
        }
    }
}