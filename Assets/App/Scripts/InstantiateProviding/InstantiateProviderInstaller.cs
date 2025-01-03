using UnityEngine;
using Zenject;

namespace App.InstantiateProviding
{
    public class InstantiateProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var instantiateProvider = new GameObject {
                transform = { name = nameof(InstantiateProvider) }
            }.AddComponent<InstantiateProvider>();

            Container.BindInterfacesAndSelfTo<InstantiateProvider>().FromInstance(instantiateProvider).AsSingle();
        }
    }
}