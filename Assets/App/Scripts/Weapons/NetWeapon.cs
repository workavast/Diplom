using System.Collections.Generic;
using App.Entities;
using App.Weapons.FSM;
using App.Weapons.Shooting;
using App.Weapons.View;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;
using Zenject;

namespace App.Weapons
{
    [RequireComponent(typeof(StateMachineController))]
    public class NetWeapon : NetworkBehaviour, IStateMachineOwner
    {
        [SerializeField] private NetWeaponModel netWeaponModel;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private WeaponViewHolder weaponViewHolder;
        
        [Inject] private readonly WeaponsConfigs _weaponsConfigs;
        [Inject] private readonly ShooterFactory _shooterFactory;
        
        public bool CanShot => netWeaponModel.NetMagazine > 0;
        public bool CanReload => netWeaponModel.NetMagazine < WeaponConfig.MagazineSize && netWeaponModel.NetReloadTimer.ExpiredOrNotRunning(Runner);
        public bool RequiredReload => netWeaponModel.NetMagazine <= 0 && CanReload;
        public WeaponId NetEquippedWeapon => netWeaponModel.NetEquippedWeapon;

        private WeaponConfig WeaponConfig => netWeaponModel.WeaponConfig;
        private Shooter Shooter => netWeaponModel.Shooter;
        
        [SerializeField, ReadOnly] private int _visibleFireCount;
        
        private WeaponStateMachine _fsm;
        private ReloadingState _reloadingState;
        private ShotReadyState _shotReadyState;

        void IStateMachineOwner.CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _reloadingState = new ReloadingState(netWeaponModel, weaponViewHolder);
            _shotReadyState = new ShotReadyState(netWeaponModel, weaponViewHolder);
            
            _fsm = new WeaponStateMachine("Weapon", _shotReadyState, _reloadingState);

            stateMachines.Add(_fsm);            
        }
        
        public override void Spawned()
        {
            netWeaponModel.Shooter = _shooterFactory.CreateShoot(GetComponent<IEntity>());
            
            SetWeapon(netWeaponModel.NetEquippedWeapon, true);
            netWeaponModel.OnEquippedWeaponChanged += SetWeapon;

            _visibleFireCount = netWeaponModel.NetFireCount;
        }

        public override void FixedUpdateNetwork()
        {
            if (_fsm.ActiveStateId == _reloadingState.StateId) 
                _fsm.TryActivateState(_shotReadyState);
        }

        public override void Render()
        {
            if (_visibleFireCount < netWeaponModel.NetFireCount)
            {
                weaponViewHolder.ShotVfx();
                weaponViewHolder.ShotSfx();
                for (int i = _visibleFireCount; i < netWeaponModel.NetFireCount; i++)
                {
                    var data = netWeaponModel.NetProjectileData[i % netWeaponModel.NetProjectileData.Length];
                    Shooter.ShootView(ref data);
                }

                _visibleFireCount = netWeaponModel.NetFireCount;
            }
        }

        public void SetWeapon(WeaponId weaponId) 
            => SetWeapon(weaponId, false);

        private void SetWeapon(WeaponId weaponId, bool force)
        {
            if (!force && netWeaponModel.NetEquippedWeapon == weaponId)
            {
                Debug.LogWarning($"You try set weapon that already setted: [{netWeaponModel.NetEquippedWeapon}] [{weaponId}]");
                return;
            }
                
            netWeaponModel.NetEquippedWeapon = weaponId;
         
            netWeaponModel.WeaponConfig = _weaponsConfigs.WeaponConfigs[weaponId];
            Shooter.SetData(shootPoint, netWeaponModel.WeaponConfig);
            
            netWeaponModel.NetFireRatePause = TickTimer.CreateFromSeconds(Runner, 0);
            netWeaponModel.NetReloadTimer = TickTimer.CreateFromSeconds(Runner, 0);
            netWeaponModel.NetMagazine = WeaponConfig.MagazineSize;
            Debug.Log($"SetWeapon: {Object.InputAuthority} | {weaponId} | {netWeaponModel.NetEquippedWeapon}");
        }

        public bool TryShoot() 
            => _fsm.ActiveState.TryShot();

        public void TryReload() 
            => _fsm.TryActivateState(_reloadingState);

        public void OnDrawGizmos() 
            => Shooter?.OnDrawGizmos();
    }
}