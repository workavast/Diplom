using Zenject;

namespace App.DiProviding
{
    public class DiProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DiProvider>().AsSingle().NonLazy();
        }
    }
}
