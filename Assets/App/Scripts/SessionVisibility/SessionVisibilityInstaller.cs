using Zenject;

namespace App.SessionVisibility
{
    public class SessionVisibilityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SessionVisibilityManager>().FromNew().AsSingle().NonLazy();
        }
    }
}