using Zenject;

namespace App.Quitting
{
    public class QuitProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<QuitProvider>().FromNew().AsSingle();
        }
    }
}