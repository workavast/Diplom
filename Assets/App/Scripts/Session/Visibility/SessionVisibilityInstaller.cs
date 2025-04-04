using Zenject;

namespace App.Session.Visibility
{
    public class SessionVisibilityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SessionVisibilityManager>().FromNew().AsSingle().NonLazy();
        }
    }
}