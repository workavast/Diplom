using Fusion;
using UnityEngine;
using Zenject;

namespace App.NetworkRunning
{
    public class NetworkRunnerInstaller : MonoInstaller
    {
        [SerializeField] private NetworkRunner networkRunnerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NetworkRunnerProvider>().FromNew().AsSingle().WithArguments(networkRunnerPrefab);
        }
    }
}