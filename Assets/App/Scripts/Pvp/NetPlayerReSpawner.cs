using System;
using App.Entities.Player;
using App.Players;
using App.Pvp.Gameplay;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Pvp
{
    public class NetPlayerReSpawner : NetworkBehaviour
    {
        [SerializeField] private float spawnDelay = 5f;
        
        [Networked] private TickTimer SpawnDelay { get; set; }
        [Networked] private bool PlayerIsAlive { get; set; }

        [Inject] private readonly GameplaySessionDataRepository _gameplaySessionDataRepository;

        private PlayerRef PlayerRef => Object.InputAuthority;
        
        private PlayerSpawnPointsProvider _playerSpawnPointsProvider;
        private NetPlayerEntity _netPlayerEntity;
        private PlayerSpawner _playerSpawner;
        private bool _isInitialized;
        
        public event Action<NetPlayerEntity> OnPlayerSpawned;
        
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
            if (_netPlayerEntity != null) 
                runner.Despawn(_netPlayerEntity.Object);
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
            _netPlayerEntity.OnDeath -= OnPlayerDeath;
            PrepareSpawn();
        }
        
        private void PrepareSpawn() 
            => SpawnDelay = TickTimer.CreateFromSeconds(Runner, spawnDelay);
        
        private void SpawnPlayer()
        {
            var weaponId = _gameplaySessionDataRepository.GetData(PlayerRef).SelectedWeapon;
            var armorLevel = _gameplaySessionDataRepository.GetData(PlayerRef).EquippedArmorLevel;
            
            _netPlayerEntity = _playerSpawner.Spawn(Object.InputAuthority, 
                _playerSpawnPointsProvider.GetRandomFreeSpawnPoint(), armorLevel, weaponId);
            _netPlayerEntity.OnDeath += OnPlayerDeath;
            PlayerIsAlive = true;
            OnPlayerSpawned?.Invoke(_netPlayerEntity);
        }
    }
}