using Zenject;

namespace App.PlayerEntities.Shooting
{
    public class ShootFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShooterFactory>().FromNew().AsSingle();
        }
    }
}