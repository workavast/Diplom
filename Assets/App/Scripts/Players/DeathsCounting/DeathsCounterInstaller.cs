using Zenject;

namespace BlackRed.Game.Players.DeathsCounting
{
    public class DeathsCounterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DeathsCounter>().FromNew().AsSingle().NonLazy();
        }
    }
}