using Zenject;

namespace App.Players.KillsCounting
{
    public class KillsCounterInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<KillsCounter>().FromNew().AsSingle().NonLazy();
        }
    }
}