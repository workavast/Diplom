using Zenject;

namespace App.Coop.Gameplay
{
    public class CoopSessionDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CoopSessionDataRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}