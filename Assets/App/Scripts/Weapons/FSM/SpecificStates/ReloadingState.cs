using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Weapons.FSM
{
    public class ReloadingState : WeaponState
    {
        public bool CanReload => NetWeaponModel.NetMagazine < WeaponConfig.MagazineSize && NetWeaponModel.NetReloadTimer.ExpiredOrNotRunning(Runner);
        
        private readonly WeaponViewHolder _weaponViewHolder;

        public ReloadingState(NetWeaponModel netWeaponModel, WeaponViewHolder weaponViewHolder) 
            : base(netWeaponModel)
        {
            _weaponViewHolder = weaponViewHolder;
        }
        
        protected override bool CanEnterState() 
            => CanReload;

        protected override bool CanExitState(IState nextState) 
            => NetWeaponModel.NetReloadTimer.ExpiredOrNotRunning(Runner);

        protected override void OnEnterState()
        {
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

        protected override void OnEnterStateRender()
        {
            var duration = NetWeaponModel.WeaponConfig.ReloadTime;
            var remainingTime = NetWeaponModel.NetReloadTimer.RemainingTime(Runner).Value;
            var initTime = 1 - remainingTime / duration;
            _weaponViewHolder.Reloading(duration, initTime);
        }
    }
}