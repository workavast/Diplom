using App.NetworkRunning.Shutdowners;
using App.NetworkRunning.Shutdowners.LocalShutdowners;
using Avastrad.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private readonly ShutdownerProvider _shutdownerProvider;

        private void Start()
        {
            _shutdownerProvider.SetLocalShutdownProvider(new DefaultShutdowner(_sceneLoader));
            _sceneLoader.HideLoadScreen(false);
        }
    }
}