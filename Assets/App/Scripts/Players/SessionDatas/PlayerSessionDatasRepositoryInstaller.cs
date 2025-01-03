using Zenject;

namespace BlackRed.Game.Players.SessionDatas
{
    public class PlayerSessionDatasRepositoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerSessionDatasRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}