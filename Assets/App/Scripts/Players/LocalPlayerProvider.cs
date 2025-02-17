using System;
using App.NetworkRunning;
using App.PlayerEntities;
using Fusion;

namespace App.Players
{
    public class LocalPlayerProvider
    {
        private readonly PlayersRepository _playersRepository;
        private readonly NetworkRunnerProvider _networkRunnerProvider;

        private NetPlayerController _netPlayerController;

        public event Action OnWeaponShot; 
        
        public LocalPlayerProvider(PlayersRepository playersRepository, NetworkRunnerProvider networkRunnerProvider)
        {
            _playersRepository = playersRepository;
            _networkRunnerProvider = networkRunnerProvider;

            _playersRepository.OnPlayerAdd += PlayerAdded;
            _playersRepository.OnPlayerRemove += PlayerRemove;
        }

        private void PlayerAdded(PlayerRef playerRef, NetPlayerController netPlayerController)
        {
            if (_networkRunnerProvider.GetNetworkRunner().LocalPlayer != playerRef)
                return;

            PlayerRemove(playerRef);
            
            _netPlayerController = netPlayerController;
            _netPlayerController.OnWeaponShot += WeaponShot;
        }
        
        private void PlayerRemove(PlayerRef playerRef)
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