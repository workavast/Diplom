using Zenject;

namespace App.Pvp.Gameplay
{
    public class GameplaySessionDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameplaySessionDataRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}