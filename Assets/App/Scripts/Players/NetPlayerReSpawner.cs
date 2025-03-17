using System;
using App.Entities.Player;
using App.Players.SessionData.Gameplay;
using App.Weapons;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players
{
    public class NetPlayerReSpawner : NetworkBehaviour
    {
        [SerializeField] private float spawnDelay = 5f;
        
        [Networked] private TickTimer SpawnDelay { get; set; }
        [Networked] private bool PlayerIsAlive { get; set; }

        [Inject] private readonly GameplaySessionDataRepository _gameplaySessionDataRepository;

        private PlayerRef PlayerRef => Object.InputAuthority;
        
        private PlayerSpawnPointsProvider _playerSpawnPointsProvider;
        private NetPlayerController _netPlayerController;
        private PlayerSpawner _playerSpawner;
        private bool _isInitialized;
        
        public event Action<NetPlayerController> OnPlayerSpawned;
        
        public void Initialize(PlayerSpawnPointsProvider playerSpawnPointsProvider, PlayerSpawner playerSpawner)
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
            if (HasStateAuthority && _isInitialized) 
                SpawnPlayer();
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
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
            PrepareSpawn();
        }
        
        private void PrepareSpawn() 
            => SpawnDelay = TickTimer.CreateFromSeconds(Runner, spawnDelay);
        
        private void SpawnPlayer()
        {
            // var weaponId = _gameplaySessionDataRepository.GetData(PlayerRef).SelectedWeapon;
            var weaponId = WeaponId.Pistol;
            
            _netPlayerController = _playerSpawner.Spawn(Object.InputAuthority, 
                _playerSpawnPointsProvider.GetRandomFreeSpawnPoint(), weaponId);
            _netPlayerController.OnDeath += OnPlayerDeath;
            PlayerIsAlive = true;
            OnPlayerSpawned?.Invoke(_netPlayerController);
        }
    }
}