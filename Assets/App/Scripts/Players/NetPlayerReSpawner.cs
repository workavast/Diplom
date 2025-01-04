using System;
using App.PlayerEntities;
using Fusion;
using UnityEngine;

namespace App.Players
{
    public class NetPlayerReSpawner : NetworkBehaviour
    {
        [SerializeField] private float spawnDelay = 5f;
        
        [Networked] private TickTimer SpawnDelay { get; set; }
        [Networked] private bool PlayerIsAlive { get; set; }
        
        private PlayerSpawnPointsProvider _playerSpawnPointsProvider;
        private NetPlayerController _netPlayerController;
        private NetPlayerSpawner _playerSpawner;
        private bool _isInitialized;
        
        public event Action<NetPlayerController> OnPlayerSpawned;
        
        public void Initialize(PlayerSpawnPointsProvider playerSpawnPointsProvider, NetPlayerSpawner playerSpawner)
        {
            if (_isInitialized)
                return;

            _playerSpawnPointsProvider = playerSpawnPointsProvider;
            _playerSpawner = playerSpawner;
            _isInitialized = true;
         
            if (HasStateAuthority) 
                SpawnPlayer();
        }
        
        public override void Spawned()
        {
            base.Spawned();

            if (HasStateAuthority && _isInitialized) 
                SpawnPlayer();
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);

            if (_netPlayerController != null) 
                runner.Despawn(_netPlayerController.Object);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;

            if (!PlayerIsAlive && SpawnDelay.ExpiredOrNotRunning(Runner)) 
                SpawnPlayer();
        }

        public void OnPlayerDeath()
        {
            if (!HasStateAuthority)
                return;

            PlayerIsAlive = false;
            _netPlayerController.OnDeath -= OnPlayerDeath;
            Runner.Despawn(_netPlayerController.Object);
            PrepareSpawn();
        }
        
        private void PrepareSpawn() 
            => SpawnDelay = TickTimer.CreateFromSeconds(Runner, spawnDelay);
        
        private void SpawnPlayer()
        {
            _netPlayerController = _playerSpawner.Spawn(Object.InputAuthority, _playerSpawnPointsProvider.GetRandomFreeSpawnPoint());
            _netPlayerController.OnDeath += OnPlayerDeath;
            PlayerIsAlive = true;
            OnPlayerSpawned?.Invoke(_netPlayerController);
        }
    }
}