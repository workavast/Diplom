using Avastrad.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader.HideLoadScreen(false);
        }
    }
}