using App.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader.Initialize(false);
        }
    }
}