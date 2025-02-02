using Avastrad.ScenesLoading;
using UnityEngine;
using Zenject;

namespace App.Bootstraps
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader.HideLoadScreen(true);
        }
    }
}