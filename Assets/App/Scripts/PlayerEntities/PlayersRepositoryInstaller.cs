using Zenject;

namespace BlackRed.Game.PlayerEntities
{
    public class PlayersRepositoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersRepository>().FromNew().AsSingle();
        }
    }
}