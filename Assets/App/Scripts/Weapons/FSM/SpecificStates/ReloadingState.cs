using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Weapons.FSM
{
    public class ReloadingState : WeaponState
    {
        public bool CanReload => NetWeaponModel.NetMagazine < WeaponConfig.MagazineSize && NetWeaponModel.NetReloadTimer.ExpiredOrNotRunning(Runner);

        public ReloadingState(NetWeaponModel netWeaponModel) 
            : base(netWeaponModel) { }
        
        protected override bool CanEnterState() 
            => CanReload;

        protected override bool CanExitState(IState nextState) 
            => NetWeaponModel.NetReloadTimer.ExpiredOrNotRunning(Runner);

        protected override void OnEnterState()
        {
            Debug.Log($"ReloadingState: {Runner.Tick} | {Runner.IsSimulationUpdating} | {Runner.IsFirstTick}");
            NetWeaponModel.NetMagazine = 0;
            NetWeaponModel.NetReloadTimer = TickTimer.CreateFromSeconds(Runner, WeaponConfig.ReloadTime);
        }
        
        protected override void OnExitState()
        {
            if (NetWeaponModel.NetReloadTimer.Expired(Runner)) 
                NetWeaponModel.NetMagazine = WeaponConfig.MagazineSize;
        }
        
        public override bool TryShot()
        {
            Debug.LogWarning("You try shot while reload weapon");
            return false;
        }
    }
}