using App.Coop;
using App.NetworkRunning;
using App.NetworkRunning.Shutdowners;
using App.NetworkRunning.Shutdowners.LocalShutdowners;
using App.Session;
using App.Session.Creation;
using App.Session.Visibility;
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
        [Inject] private readonly SessionVisibilityManager _sessionVisibilityManager;
        [Inject] private readonly ShutdownerProvider _shutdownerProvider;

        private async void Start()
        {
            if (!_runnerProvider.TryGetNetworkRunner(out _))
                await _sessionCreator.CreateSinglePlayer(SceneManager.GetActiveScene().buildIndex);

            _sessionVisibilityManager.SetHardVisibility(false);
            _shutdownerProvider.SetLocalShutdownProvider(new DefaultShutdowner(_sceneLoader));
            
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