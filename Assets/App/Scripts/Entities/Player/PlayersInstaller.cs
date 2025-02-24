using App.Entities.Enemy;
using App.Players;
using Zenject;

namespace App.Entities.Player
{
    public class PlayersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersEntitiesRepository>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesRepository>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<EntitiesRepository>().FromNew().AsSingle();
            
            Container.Bind<LocalPlayerProvider>().FromNew().AsSingle();
        }
    }
}