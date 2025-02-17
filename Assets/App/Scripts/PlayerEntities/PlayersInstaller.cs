using App.Enemy;
using App.Entities;
using App.Players;
using Zenject;

namespace App.PlayerEntities
{
    public class PlayersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersRepository>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesRepository>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<EntitiesRepository>().FromNew().AsSingle();
            
            Container.Bind<LocalPlayerProvider>().FromNew().AsSingle();
        }
    }
}