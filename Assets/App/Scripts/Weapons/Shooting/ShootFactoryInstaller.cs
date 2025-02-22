using Zenject;

namespace App.Weapons.Shooting
{
    public class ShootFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShooterFactory>().FromNew().AsSingle();
        }
    }
}