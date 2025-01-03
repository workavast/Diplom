using Zenject;

namespace BlackRed.Game.Quitting
{
    public class QuitProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<QuitProvider>().FromNew().AsSingle();
        }
    }
}