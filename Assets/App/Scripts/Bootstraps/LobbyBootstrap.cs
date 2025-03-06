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
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private NetworkRunnerProvider _runnerProvider;
        [Inject] private SessionCreator _sessionCreator;
        
        private async void Start()
        {
            if (!_runnerProvider.TryGetNetworkRunner(out _))
                await _sessionCreator.CreateSinglePlayer(SceneManager.GetActiveScene().buildIndex);
            
            _sceneLoader.HideLoadScreen(true);
        }
    }
}