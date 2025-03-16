using UnityEngine;
using Zenject;

namespace App.Lobby.StartGameTimer
{
    public class StartGameTimerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var netStartGameTimerModel = FindAnyObjectByType<NetStartGameTimerModel>(FindObjectsInactive.Include);
            Container.Bind<IReadOnlyGameStartTimer>().FromInstance(netStartGameTimerModel).AsSingle();
        }
    }
}