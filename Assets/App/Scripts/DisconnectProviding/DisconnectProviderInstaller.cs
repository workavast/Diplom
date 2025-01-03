using Zenject;

namespace App.DisconnectProviding
{
    public class DisconnectProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DisconnectProvider>().FromNew().AsSingle();
        }
    }
}