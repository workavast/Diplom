using App.PlayerEntities;
using App.Weapons;
using Fusion;
using UnityEngine;

namespace App.Players
{
    public class NetPlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private NetPlayerController netPlayerControllerPrefab;

        public NetPlayerController Spawn(PlayerRef playerRef, Transform spawnPoint, WeaponId initialWeapon = WeaponId.None)
            => Spawn(playerRef, spawnPoint.position, spawnPoint.rotation, initialWeapon);
        
        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, WeaponId initialWeapon = WeaponId.None)
            => Spawn(playerRef, position, Quaternion.identity, initialWeapon);

        public NetPlayerController Spawn(PlayerRef playerRef, Vector3 position, Quaternion rotation, WeaponId initialWeapon = WeaponId.None)
        {
            var netPlayerController = Runner.Spawn(netPlayerControllerPrefab, position, rotation, playerRef);
            netPlayerController.SetWeapon(initialWeapon);
            return netPlayerController;
        }
    }
}