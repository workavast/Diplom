using Zenject;

namespace App.Pvp.DeathsCounting
{
    public class DeathsCounterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DeathsCounter>().FromNew().AsSingle().NonLazy();
        }
    }
}