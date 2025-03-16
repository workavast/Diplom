using System;
using App.NetworkRunning;
using Avastrad.ScenesLoading;
using Fusion;
using UnityEngine.SceneManagement;
using Zenject;

namespace App.ScenesLoading
{
    public class NetScenesLoader : ISceneLoader
    {
        private readonly int _loadingSceneIndex;

        private readonly ILoadingScreen _loadingScreen;
        private readonly NetworkRunnerProvider _networkRunnerProvider;
        private int _targetSceneIndex = -1;
        
        public event Action OnLoadingStarted;
        public event Action OnLoadingScreenHided;

        [Inject]
        public NetScenesLoader(ILoadingScreen loadingScreen, int loadingSceneIndex, NetworkRunnerProvider networkRunnerProvider)
        {
            _loadingScreen = loadingScreen;
            _loadingSceneIndex = loadingSceneIndex;
            _networkRunnerProvider = networkRunnerProvider;
            
            _loadingScreen.OnHided += () => OnLoadingScreenHided?.Invoke();
        }
        
        public void HideLoadScreen(bool hideLoadScreenInstantly) 
            => _loadingScreen.Hide(hideLoadScreenInstantly);

        /// <param name="forceLoading"> skip loading scene </param>
        public void LoadScene(int index, bool showLoadScreenInstantly = false, bool forceLoading = false)
        {
            _targetSceneIndex = index;
            
            OnLoadingStarted?.Invoke();

            if (forceLoading)
            {
                var netRunner = _networkRunnerProvider.GetNetworkRunner();
                if (netRunner.IsRunning && !netRunner.IsShutdown)
                    netRunner.LoadScene(SceneRef.FromIndex(_targetSceneIndex));
                else
                    SceneManager.LoadSceneAsync(_loadingSceneIndex);
            }
            else
            {
                _loadingScreen.Show(showLoadScreenInstantly, 
                    () =>
                    {
                        var netRunner = _networkRunnerProvider.GetNetworkRunner();
                        if (netRunner.IsRunning && !netRunner.IsShutdown)
                            netRunner.LoadScene(SceneRef.FromIndex(_loadingSceneIndex));
                        else
                            SceneManager.LoadSceneAsync(_loadingSceneIndex);
                    });  
            }
        }

        public void LoadTargetScene()
        {
            var netRunner = _networkRunnerProvider.GetNetworkRunner();

            if (netRunner.IsRunning && !netRunner.IsServer)
                return;

            if (netRunner.IsRunning) 
                netRunner.LoadScene(SceneRef.FromIndex(_targetSceneIndex));
            else
                SceneManager.LoadSceneAsync(_targetSceneIndex);
        }
    }
}