using App.CursorBehaviour;
using App.NetworkRunning.Shutdowners;
using App.NetworkRunning.Shutdowners.LocalShutdowners;
using Avastrad.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class AppBootstrap : MonoBehaviour
    {
        [SerializeField] private int sceneIndexForLoadingAfterInitializations = 2;
        
        [Inject] private readonly ISceneLoader _sceneLoader;
        [Inject] private readonly CursorVisibilityBehaviour _cursorVisibilityBehaviour;
        [Inject] private readonly ShutdownerProvider _shutdownerProvider;
        
        private void Awake()
        {
            _cursorVisibilityBehaviour.CheckCursorVisibilityState();
        }

        private void Start()
        {
            _shutdownerProvider.SetLocalShutdownProvider(new DefaultShutdowner(_sceneLoader));
            _sceneLoader.LoadScene(sceneIndexForLoadingAfterInitializations, true);
        }
    }
}