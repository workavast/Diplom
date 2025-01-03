using System;
using BlackRed.Game.NetworkRunning;
using Fusion;
using UnityEngine.SceneManagement;
using Zenject;

namespace BugStrategy.ScenesLoading
{
    public class SceneLoader : ISceneLoader
    {
        private static int BootstrapSceneIndex => ScenesConfig.BootstrapSceneIndex;
        private static int LoadingSceneIndex => ScenesConfig.LoadingSceneIndex;
        
        private readonly ILoadingScreen _loadingScreen;
        private readonly NetworkRunnerProvider _networkRunnerProvider;
        private int _targetSceneIndex = -1;
        
        public event Action OnLoadingStarted;
        public event Action OnLoadingScreenHided;

        [Inject]
        public SceneLoader(ILoadingScreen loadingScreen, NetworkRunnerProvider networkRunnerProvider)
        {
            _loadingScreen = loadingScreen;
            _networkRunnerProvider = networkRunnerProvider;
            
            _loadingScreen.OnHided += () => OnLoadingScreenHided?.Invoke();
        }
        
        public void Initialize(bool endInstantly)
        {
            var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            if(activeSceneIndex == BootstrapSceneIndex)
                return;
            
            if (_targetSceneIndex <= -1 || activeSceneIndex == _targetSceneIndex)
                EndLoading(endInstantly);

            if (activeSceneIndex == LoadingSceneIndex)
                StartLoadTargetScene();
        }
        
        public void LoadScene(int index)
        {
            if (index == BootstrapSceneIndex)
                throw new ArgumentOutOfRangeException($"You cant load bootstrap scene");
            if (index == LoadingSceneIndex)
                throw new ArgumentOutOfRangeException($"You cant load loading scene");

            _targetSceneIndex = index;
            
            OnLoadingStarted?.Invoke();
            
            _loadingScreen.Show();

            var netRunner = _networkRunnerProvider.GetNetworkRunner();
            if (netRunner.IsRunning) 
                netRunner.LoadScene(SceneRef.FromIndex(LoadingSceneIndex));
            else
                SceneManager.LoadSceneAsync(LoadingSceneIndex);
        }

        private void EndLoading(bool endInstantly) 
            => _loadingScreen.Hide(endInstantly);

        private void StartLoadTargetScene()
        {
            var netRunner = _networkRunnerProvider.GetNetworkRunner();
            if (netRunner.IsRunning) 
                netRunner.LoadScene(SceneRef.FromIndex(_targetSceneIndex));
            else
                SceneManager.LoadSceneAsync(_targetSceneIndex);
        }
    }
}