using System;
using System.Threading.Tasks;
using BlackRed.Game.InstantiateProviding;
using Fusion;
using UnityEngine;
using Zenject;

namespace BlackRed.Game.NetworkRunning
{
    public class NetworkRunnerProvider
    {
        private readonly IInstantiateProvider _instantiateProvider;
        private readonly NetworkRunner _networkRunnerPrefab;
        private readonly DiContainer _diContainer;

        private bool _isStartInvoked;
        private NetworkRunner _networkRunner;

        public NetworkRunnerProvider(IInstantiateProvider instantiateProvider, NetworkRunner networkRunnerPrefab, 
            DiContainer diContainer)
        {
            _instantiateProvider = instantiateProvider;
            _networkRunnerPrefab = networkRunnerPrefab;
            _diContainer = diContainer;
        }

        public Task<StartGameResult> StartGame(bool provideInput, StartGameArgs args)
        {
            if (_isStartInvoked && _networkRunner != null)
                throw new Exception("You try start game, when it started");

            _isStartInvoked = true;
            
            if (_networkRunner == null) 
                InstantiateNetworkRunner();

            _networkRunner.ProvideInput = true;
             return _networkRunner.StartGame(args);
        }
        
        public NetworkRunner GetNetworkRunner()
        {
            if (_networkRunner == null) 
                InstantiateNetworkRunner();

            return _networkRunner;
        }
        
        public bool TryGetNetworkRunner(out NetworkRunner networkRunner)
        {
            networkRunner = _networkRunner;
            return _networkRunner != null || _isStartInvoked && _networkRunner.IsShutdown;
        }

        public async void Shutdown()
        {
            await _networkRunner.Shutdown();
            await Task.Delay(100);
        }
        
        private void InstantiateNetworkRunner()
        {
            if (_networkRunner != null)
            {
                Debug.LogError("_network runner exist");
                return;
            }
            
            _isStartInvoked = false;
            _networkRunner = _instantiateProvider.Instantiate(_networkRunnerPrefab);
            _diContainer.InjectGameObject(_networkRunner.gameObject);
        }
    }
}