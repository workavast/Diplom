using App.Lobby;
using App.Lobby.StartGameTimer;
using App.NetworkRunning;
using App.Session;
using Avastrad.ScenesLoading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace App.Bootstraps
{
    public class LobbyBootstrap : MonoBehaviour
    {
        [Inject] private readonly ISceneLoader _sceneLoader;
        [Inject] private readonly NetworkRunnerProvider _runnerProvider;
        [Inject] private readonly SessionCreator _sessionCreator;
        [Inject] private readonly IReadOnlyGameStartTimer _gameStartTimer;
        
        private GameStarter _gameStarter;
        
        private async void Start()
        {
            if (!_runnerProvider.TryGetNetworkRunner(out _))
                await _sessionCreator.CreateSinglePlayer(SceneManager.GetActiveScene().buildIndex);
            
            _sceneLoader.HideLoadScreen(true);

            if (_runnerProvider.TryGetNetworkRunner(out var runner) && runner.IsServer)
                _gameStarter = new GameStarter(_gameStartTimer, _sceneLoader);
        }
    }
}