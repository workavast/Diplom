using Zenject;

namespace App.Pvp.KillsCounting
{
    public class KillsCounterInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<KillsCounter>().FromNew().AsSingle().NonLazy();
        }
    }
}