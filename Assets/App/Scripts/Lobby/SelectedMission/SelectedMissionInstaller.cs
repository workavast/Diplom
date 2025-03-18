using UnityEngine;
using Zenject;

namespace App.Lobby.SelectedMission
{
    public class SelectedMissionInstaller : MonoInstaller
    {
        [SerializeField] private NetLobbyData netLobbyData;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NetLobbyData>().FromInstance(netLobbyData).AsSingle();
        }
    }
}