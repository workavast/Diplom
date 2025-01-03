using Zenject;

namespace App.Players.SessionDatas
{
    public class PlayerSessionDatasRepositoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerSessionDatasRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}