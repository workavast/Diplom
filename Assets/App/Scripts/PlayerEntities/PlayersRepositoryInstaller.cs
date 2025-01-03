using Zenject;

namespace App.PlayerEntities
{
    public class PlayersRepositoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersRepository>().FromNew().AsSingle();
        }
    }
}