using System;
using App.Players;
using App.Weapons;
using Fusion;
using Zenject;

namespace App.Coop.Gameplay
{
    public class NetCoopSessionData : NetworkBehaviour
    {
        [Networked] [field: ReadOnly] public bool Initialized { get; private set; }
        [Networked] [field: ReadOnly] public WeaponId SelectedWeapon { get; private set; }
        [Networked] [field: ReadOnly] public int EquippedArmorLevel { get; private set; }

        [Inject] private CoopSessionDataRepository _playersSessionDataRepository;
        
        public PlayerRef PlayerRef => Object.InputAuthority;
        
        public event Action OnDespawned;

        public override void Spawned()
        {
            if (HasInputAuthority)
                RPC_InitializeData(PlayerData.SelectedWeapon, PlayerData.EquippedArmorLevel);
            
            if (!_playersSessionDataRepository.TryRegister(PlayerRef, this))
                Runner.Despawn(GetComponent<NetworkObject>());
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersSessionDataRepository.TryRemove(PlayerRef);
            OnDespawned?.Invoke();
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_InitializeData(WeaponId weapon, int armorLevel)
        {
            SelectedWeapon = weapon;
            EquippedArmorLevel = armorLevel;
        }
    }
}