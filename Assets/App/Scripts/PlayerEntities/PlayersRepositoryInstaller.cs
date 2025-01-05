using App.Enemy;
using App.Entities;
using Zenject;

namespace App.PlayerEntities
{
    public class PlayersRepositoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersRepository>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesRepository>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<EntitiesRepository>().FromNew().AsSingle();
        }
    }
}