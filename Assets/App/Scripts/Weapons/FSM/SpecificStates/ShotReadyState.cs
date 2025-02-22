using Fusion;

namespace App.Weapons.FSM
{
    public class ShotReadyState : WeaponState
    {
        private readonly WeaponViewHolder _weaponViewHolder;

        public ShotReadyState(NetWeaponModel netWeapon, WeaponViewHolder weaponViewHolder) 
            : base(netWeapon)
        {
            _weaponViewHolder = weaponViewHolder;
        }

        public override bool TryShot()
        {
            if (NetWeaponModel.NetMagazine <= 0)
                return false;
            
            if (!NetWeaponModel.NetFireRatePause.Expired(Runner))
                return false;

            var fireRatePause = 60 / WeaponConfig.FireRate;
            NetWeaponModel.NetFireRatePause = TickTimer.CreateFromSeconds(Runner, fireRatePause);
            NetWeaponModel.NetMagazine--;
            
            var isHit = NetWeaponModel.Shooter.Shoot(NetWeaponModel.HasStateAuthority, out var data, NetWeaponModel.HitLayers);
            if (isHit)
            {
                NetWeaponModel.NetProjectileData.Set(NetWeaponModel.NetFireCount % NetWeaponModel.NetProjectileData.Length, data);
                NetWeaponModel.NetFireCount++;
            }

            return true;
        }
        
        protected override void OnEnterStateRender() 
            => _weaponViewHolder.Default();
    }
}