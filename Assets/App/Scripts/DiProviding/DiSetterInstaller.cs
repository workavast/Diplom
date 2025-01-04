using Zenject;

namespace App.DiProviding
{
    public class DiSetterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DiSetter>().FromNew().AsSingle().NonLazy();
        }
    }
}