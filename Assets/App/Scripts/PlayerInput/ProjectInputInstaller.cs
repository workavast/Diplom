using App.PlayerInput.InputProviding;
using UnityEngine;
using Zenject;

namespace App.PlayerInput
{
    public class ProjectInputInstaller : MonoInstaller
    {
        [SerializeField] private RawInputProvider rawInputProvider;
        
        public override void InstallBindings()
        {
            Container.Bind<RawInputProvider>().FromInstance(rawInputProvider).AsSingle();
            Container.BindInterfacesAndSelfTo<MainInputProvider>().FromNew().AsSingle().WithArguments(new DefaultInputProvider(rawInputProvider));
        }
    }
}