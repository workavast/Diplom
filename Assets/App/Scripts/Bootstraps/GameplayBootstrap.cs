using App.Coop;
using App.NetworkRunning;
using App.Session;
using Avastrad.ScenesLoading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace App.Bootstraps
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private NetPlayersReady netPlayersReady;
  
        [Inject] private readonly ISceneLoader _sceneLoader;
        [Inject] private readonly NetworkRunnerProvider _runnerProvider;
        [Inject] private readonly SessionCreator _sessionCreator;
        
        private async void Start()
        {
            if (!_runnerProvider.TryGetNetworkRunner(out _))
                await _sessionCreator.CreateSinglePlayer(SceneManager.GetActiveScene().buildIndex);

            if (netPlayersReady.AllPlayersIsReady)
                OnAllPlayersReady();
            else
                netPlayersReady.OnAllPlayersIsReady += OnAllPlayersReady;
        }

        private void OnAllPlayersReady()
        {
            netPlayersReady.OnAllPlayersIsReady -= OnAllPlayersReady;
            _sceneLoader.HideLoadScreen(true);
        }
    }
}