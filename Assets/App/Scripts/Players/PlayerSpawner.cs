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

        public NetPlayerController Spawn(PlayerRef playerRef, Transform spawnPoint, int armorLevel, WeaponId initialWeapon = WeaponId.Pistol)
            => Spawn(playerRef, spawnPoint.position, spawnPoint.rotation, armorLevel, initialWeapon);
        
        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, int armorLevel, WeaponId initialWeapon = WeaponId.Pistol)
            => Spawn(playerRef, position, Quaternion.identity, armorLevel, initialWeapon);

        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, Quaternion rotation, int armorLevel, WeaponId initialWeapon = WeaponId.Pistol)
        {
            var runner = _runnerProvider.GetNetworkRunner();
            var netPlayerController = runner.Spawn(netPlayerControllerPrefab, position, rotation, playerRef);
            netPlayerController.SetWeapon(initialWeapon);
            netPlayerController.SetArmor(armorLevel);
            return netPlayerController;
        }
    }
}