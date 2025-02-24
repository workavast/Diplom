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

        private NetPlayerController _netPlayerController;

        public event Action OnWeaponShot; 
        
        public LocalPlayerProvider(PlayersEntitiesRepository playersEntitiesRepository, NetworkRunnerProvider networkRunnerProvider)
        {
            _playersEntitiesRepository = playersEntitiesRepository;
            _networkRunnerProvider = networkRunnerProvider;

            _playersEntitiesRepository.OnPlayerAdd += PlayerEntityAdded;
            _playersEntitiesRepository.OnPlayerRemove += PlayerEntityRemove;
        }

        private void PlayerEntityAdded(PlayerRef playerRef, NetPlayerController netPlayerController)
        {
            if (_networkRunnerProvider.GetNetworkRunner().LocalPlayer != playerRef)
                return;

            PlayerEntityRemove(playerRef);
            
            _netPlayerController = netPlayerController;
            _netPlayerController.OnWeaponShot += WeaponShot;
        }
        
        private void PlayerEntityRemove(PlayerRef playerRef)
        {
            if (_networkRunnerProvider.GetNetworkRunner().LocalPlayer != playerRef)
                return;
            if (_netPlayerController == null)
                return;

            _netPlayerController.OnWeaponShot -= WeaponShot;

            _netPlayerController = null;
        }

        private void WeaponShot() 
            => OnWeaponShot?.Invoke();
    }
}