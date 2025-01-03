using Zenject;

namespace BlackRed.Game.EventBus
{
    public class EventBusInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Avastrad.EventBusFramework.EventBus>().FromNew().AsSingle();
        }
    }
}