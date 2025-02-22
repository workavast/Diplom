using System;
using App.Weapons.Shooting;
using Fusion;
using UnityEngine;

namespace App.Weapons
{
    public class NetWeaponModel : NetworkBehaviour
    {
        [field: SerializeField] public LayerMask HitLayers { get; set; }

        [OnChangedRender(nameof(OnNetEquippedWeaponChanged))]
        [Networked] [field: SerializeField, ReadOnly] public WeaponId NetEquippedWeapon { get; set; }
        [Networked] [field: SerializeField, ReadOnly] public int NetMagazine { get; set; }
        [Networked] [field: SerializeField, ReadOnly] public int NetFireCount { get; set; }

        [Networked] public TickTimer NetFireRatePause { get; set; }
        [Networked] public TickTimer NetReloadTimer { get; set; }
        [Networked, Capacity(32)] public NetworkArray<ProjectileData> NetProjectileData { get; }

        [field: SerializeField, ReadOnly] public WeaponConfig WeaponConfig { get; set; }

        public Shooter Shooter { get; set; }
        
        public event Action<WeaponId> OnEquippedWeaponChanged;

        private void OnNetEquippedWeaponChanged() 
            => OnEquippedWeaponChanged?.Invoke(NetEquippedWeapon);
    }
}