using System;
using App.Entities.Player;
using App.NetworkRunning;
using Fusion;

namespace App.Players
{
    public class LocalPlayerProvider
    {
        private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        private readonly NetworkRunnerProvider _networkRunnerProvider;

        private NetPlayerEntity _netPlayerEntity;

        public event Action OnWeaponShot; 
        
        public LocalPlayerProvider(PlayersEntitiesRepository playersEntitiesRepository, NetworkRunnerProvider networkRunnerProvider)
        {
            _playersEntitiesRepository = playersEntitiesRepository;
            _networkRunnerProvider = networkRunnerProvider;

            _playersEntitiesRepository.OnPlayerAdd += PlayerEntityAdded;
            _playersEntitiesRepository.OnPlayerRemove += PlayerEntityRemove;
        }

        private void PlayerEntityAdded(PlayerRef playerRef, NetPlayerEntity netPlayerEntity)
        {
            if (_networkRunnerProvider.GetNetworkRunner().LocalPlayer != playerRef)
                return;

            PlayerEntityRemove(playerRef);
            
            _netPlayerEntity = netPlayerEntity;
            _netPlayerEntity.OnWeaponShot += WeaponShot;
        }
        
        private void PlayerEntityRemove(PlayerRef playerRef)
        {
            if (_networkRunnerProvider.GetNetworkRunner().LocalPlayer != playerRef)
                return;
            if (_netPlayerEntity == null)
                return;

            _netPlayerEntity.OnWeaponShot -= WeaponShot;

            _netPlayerEntity = null;
        }

        private void WeaponShot() 
            => OnWeaponShot?.Invoke();
    }
}