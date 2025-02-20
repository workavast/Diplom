using Fusion;
using UnityEngine;

namespace App.Weapons.FSM
{
    public class ShotReadyState : WeaponState
    {
        public ShotReadyState(NetWeaponModel netWeapon) 
            : base(netWeapon) { }

        protected override void OnEnterState()
        {
            Debug.Log($"ShotReadyState: {Runner.Tick} | {Runner.IsSimulationUpdating} | {Runner.IsFirstTick}");
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
    }
}