using App.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class LoadingSceneBootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;
        
        private void Start()
        {
            _sceneLoader.Initialize(false);
        }
    }
}