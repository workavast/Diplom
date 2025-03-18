using System;
using App.Lobby.SelectedMission;
using App.Lobby.StartGameTimer;
using Avastrad.ScenesLoading;

namespace App.Lobby
{
    public class GameStarter : IDisposable
    {
        private readonly IReadOnlyGameStartTimer _startGameTimer;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISelectedMissionProvider _selectedMissionProvider;

        public GameStarter(IReadOnlyGameStartTimer startGameTimer, ISceneLoader sceneLoader, ISelectedMissionProvider selectedMissionProvider)
        {
            _startGameTimer = startGameTimer;
            _sceneLoader = sceneLoader;
            _selectedMissionProvider = selectedMissionProvider;

            _startGameTimer.OnTimerIsOver += StartGame;
        }

        private void StartGame()
        {
            var missionConfig = _selectedMissionProvider.GetMission(_selectedMissionProvider.ActiveMissionIndex);
            _sceneLoader.LoadScene(missionConfig.SceneIndex);
        }

        public void Dispose()
        {
            _startGameTimer.OnTimerIsOver -= StartGame;
        }
    }
}