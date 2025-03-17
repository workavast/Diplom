using App.Entities.Player;
using App.NetworkRunning;
using App.Weapons;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private NetPlayerController netPlayerControllerPrefab;

        [Inject] private readonly NetworkRunnerProvider _runnerProvider;

        public NetPlayerController Spawn(PlayerRef playerRef, Transform spawnPoint, WeaponId initialWeapon = WeaponId.None)
            => Spawn(playerRef, spawnPoint.position, spawnPoint.rotation, initialWeapon);
        
        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, WeaponId initialWeapon = WeaponId.None)
            => Spawn(playerRef, position, Quaternion.identity, initialWeapon);

        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, Quaternion rotation, WeaponId initialWeapon = WeaponId.None)
        {
            var runner = _runnerProvider.GetNetworkRunner();
            var netPlayerController = runner.Spawn(netPlayerControllerPrefab, position, rotation, playerRef);
            netPlayerController.SetWeapon(initialWeapon);
            return netPlayerController;
        }
    }
}