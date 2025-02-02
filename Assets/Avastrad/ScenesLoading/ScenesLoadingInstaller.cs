using System;
using UnityEngine;
using Zenject;

namespace Avastrad.ScenesLoading
{
    public class ScenesLoadingInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreen loadingScreenPrefab;
        [SerializeField] private int loadingSceneIndex = 1;
            
        public override void InstallBindings()
        {
            BindLoadingScreen();
            BindSceneLoader();
        }

        private void BindLoadingScreen()
        {
            if (loadingScreenPrefab == null)
                throw new NullReferenceException($"{nameof(loadingScreenPrefab)} is null");
            
            var loadingScreen = Instantiate(loadingScreenPrefab);
            loadingScreen.Initialize();
            DontDestroyOnLoad(loadingScreen);
            Container.BindInterfacesTo<LoadingScreen>().FromInstance(loadingScreen).AsSingle();
        }
        
        private void BindSceneLoader()
        {
            Container.BindInterfacesTo<SceneLoader>().FromNew().AsSingle().WithArguments(loadingSceneIndex);
        }
    }
}