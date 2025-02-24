using Zenject;

namespace App.Players.Repository
{
    public class PlayersRepositoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}