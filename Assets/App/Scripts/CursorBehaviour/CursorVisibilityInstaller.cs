using Zenject;

namespace App.CursorBehaviour
{
    public class CursorVisibilityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CursorVisibilityBehaviour>().FromNew().AsSingle().NonLazy();
        }
    }
}