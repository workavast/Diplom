using App.NetworkRunning.Shutdowners;
using App.NetworkRunning.Shutdowners.LocalShutdowners;
using Avastrad.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class LoadingSceneBootstrap : MonoBehaviour
    {
        [Inject] private readonly ShutdownerProvider _shutdownerProvider;
        [Inject] private readonly ISceneLoader _sceneLoader;

        private void Start()
        {
            _shutdownerProvider.SetLocalShutdownProvider(new DefaultShutdowner(_sceneLoader));
            _sceneLoader.LoadTargetScene();
        }
    }
}