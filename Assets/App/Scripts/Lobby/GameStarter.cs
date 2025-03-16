using System;
using App.Lobby.StartGameTimer;
using App.ScenesLoading;
using Avastrad.ScenesLoading;

namespace App.Lobby
{
    public class GameStarter : IDisposable
    {
        private readonly IReadOnlyGameStartTimer _startGameTimer;
        private readonly ISceneLoader _sceneLoader;

        public GameStarter(IReadOnlyGameStartTimer startGameTimer, ISceneLoader sceneLoader)
        {
            _startGameTimer = startGameTimer;
            _sceneLoader = sceneLoader;
            
            _startGameTimer.OnTimerIsOver += StartGame;
        }

        private void StartGame()
        {
            _sceneLoader.LoadScene(ScenesConfig.GameplaySceneIndex);
        }

        public void Dispose()
        {
            _startGameTimer.OnTimerIsOver -= StartGame;
        }
    }
}