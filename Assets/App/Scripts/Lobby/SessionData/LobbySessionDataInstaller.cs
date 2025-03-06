using Zenject;

namespace App.Lobby.SessionData
{
    public class LobbySessionDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LobbySessionDataRepository>().FromNew().AsSingle().NonLazy();
        }
    }
}