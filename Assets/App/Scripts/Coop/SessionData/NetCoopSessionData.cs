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
        [Networked] [field: ReadOnly] [OnChangedRender(nameof(SelectedWeaponChanged))] public WeaponId SelectedWeapon { get; private set; }

        [Inject] private CoopSessionDataRepository _playersSessionDataRepository;
        
        public PlayerRef PlayerRef => Object.InputAuthority;
        
        public event Action OnDespawned;
        public event Action OnSelectedWeaponChanged;

        public override void Spawned()
        {
            if (HasInputAuthority)
                RPC_SetWeapon(PlayerData.SelectedWeapon);
            
            if (!_playersSessionDataRepository.TryRegister(PlayerRef, this))
                Runner.Despawn(GetComponent<NetworkObject>());
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersSessionDataRepository.TryRemove(PlayerRef);
            OnDespawned?.Invoke();
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_SetWeapon(WeaponId selectedWeapon) 
            => SelectedWeapon = selectedWeapon;

        private void SelectedWeaponChanged() 
            => OnSelectedWeaponChanged?.Invoke();
    }
}