using App.CursorBehaviour;
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
        
        private void Awake()
        {
            _cursorVisibilityBehaviour.CheckCursorVisibilityState();
        }

        private void Start()
        {
            _sceneLoader.LoadScene(sceneIndexForLoadingAfterInitializations, true);
        }
    }
}