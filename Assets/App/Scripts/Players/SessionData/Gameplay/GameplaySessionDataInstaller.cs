using Zenject;

namespace App.Players.SessionData.Gameplay
{
    public class GameplaySessionDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameplaySessionDataRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}