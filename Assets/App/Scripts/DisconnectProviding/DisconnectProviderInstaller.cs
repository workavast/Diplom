using Zenject;

namespace BlackRed.Game.DisconnectProviding
{
    public class DisconnectProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DisconnectProvider>().FromNew().AsSingle();
        }
    }
}