using System;
using UnityEngine.SceneManagement;

namespace Avastrad.ScenesLoading
{
    public class DefaultSceneLoader : ISceneLoader
    {
        private readonly int _loadingSceneIndex;
        
        private readonly ILoadingScreen _loadingScreen;
        private int _targetSceneIndex = -1;
        
        public event Action OnLoadingStarted;
        public event Action OnLoadingScreenHided;

        public DefaultSceneLoader(ILoadingScreen loadingScreen, int loadingSceneIndex)
        {
            _loadingScreen = loadingScreen;
            _loadingSceneIndex = loadingSceneIndex;
            
            _loadingScreen.OnHided += () => OnLoadingScreenHided?.Invoke();
        }
        
        public void ShowLoadScreen(bool showInstantly, Action onShowedCallback)
            => _loadingScreen.Show(showInstantly, onShowedCallback);
        
        public void HideLoadScreen(bool hideLoadScreenInstantly)
            => _loadingScreen.Hide(hideLoadScreenInstantly);

        public void LoadScene(int index, bool showLoadScreenInstantly = false, bool forceLoading = false)
        {
            _targetSceneIndex = index;
            
            OnLoadingStarted?.Invoke();

            if (forceLoading)
                SceneManager.LoadScene(_loadingSceneIndex);
            else
                _loadingScreen.Show(showLoadScreenInstantly, () => SceneManager.LoadSceneAsync(_loadingSceneIndex));
        }

        public void LoadTargetScene() 
            => SceneManager.LoadSceneAsync(_targetSceneIndex);
    }
}