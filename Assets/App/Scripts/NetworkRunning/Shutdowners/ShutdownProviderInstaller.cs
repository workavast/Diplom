using Zenject;

namespace App.NetworkRunning.Shutdowners
{
    public class ShutdownProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ShutdownerProvider>().FromNew().AsSingle();
        }
    }
}