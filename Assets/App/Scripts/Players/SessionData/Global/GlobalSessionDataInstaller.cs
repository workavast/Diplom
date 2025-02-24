using Zenject;

namespace App.Players.SessionData.Global
{
    public class GlobalSessionDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GlobalSessionDataRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}