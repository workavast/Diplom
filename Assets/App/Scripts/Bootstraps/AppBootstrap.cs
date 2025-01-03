using App.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class AppBootstrap : MonoBehaviour
    {
        [SerializeField] private int sceneIndexForLoadingAfterInitializations = 2;
        
        [Inject] private ISceneLoader _sceneLoader;
        
        private void Start()
        {
            _sceneLoader.Initialize(false);
            _sceneLoader.LoadScene(sceneIndexForLoadingAfterInitializations);
        }
    }
}