using System;
using App.Players;
using App.Weapons;
using Fusion;
using Zenject;

namespace App.Pvp.Gameplay
{
    public class NetGameplaySessionData : NetworkBehaviour
    {
        [Networked] [field: ReadOnly] public bool Initialized { get; private set; }
        [Networked] [field: ReadOnly] public WeaponId SelectedWeapon { get; private set; }
        [Networked] [field: ReadOnly] public int EquippedArmorLevel { get; private set; }
        [Networked] [field: ReadOnly] [OnChangedRender(nameof(PointsChanged))] public int Points { get; private set; }
        [Networked] [field: ReadOnly] [OnChangedRender(nameof(DeathsChanged))] public int Deaths { get; private set; }
        [Networked] [field: ReadOnly] [OnChangedRender(nameof(KillsChanged))] public int Kills { get; private set; }

        [Inject] private GameplaySessionDataRepository _playersSessionDataRepository;
        
        public PlayerRef PlayerRef => Object.InputAuthority;
        
        public event Action OnDespawned;
        public event Action OnPointsChanged;
        public event Action OnKillsChanged;
        public event Action OnDeathsChanged;

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

        private void PointsChanged() 
            => OnPointsChanged?.Invoke();
        
        private void KillsChanged() 
            => OnKillsChanged?.Invoke();

        private void DeathsChanged() 
            => OnDeathsChanged?.Invoke();

        public void ChangePoints(int value) 
            => Points += value;

        public void ChangeKills(int value) 
            => Kills += value;

        public void ChangeDeaths(int value) 
            => Deaths += value;
    }
}