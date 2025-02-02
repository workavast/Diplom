using Zenject;

namespace App.Players.Nicknames
{
    public class NicknamesProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NicknamesProvider>().FromNew().AsSingle();
        }
    }
}