using App.Entities;
using App.PlayerEntities.Shooting;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Weapons
{
    public class NetWeapon : NetworkBehaviour
    {
        [SerializeField] private WeaponView weaponView;
        [SerializeField] protected Transform shootPoint;

        [OnChangedRender(nameof(OnNetEquippedWeaponChanged))]
        [Networked] public WeaponId NetEquippedWeapon { get; private set; }
        [Networked] public TickTimer NetFireRatePause { get; private set; }
        [Networked] public int NetFireCount { get; set; }
        [Networked, Capacity(32)] public NetworkArray<ProjectileData> NetProjectileData { get; }

        [Inject] private readonly WeaponFactory _weaponFactory;

        private WeaponConfig _weaponConfig;
        private Shooter _shooter;
        private int _visibleFireCount;

        public override void Spawned()
        {
            SetWeapon(NetEquippedWeapon);

            if (HasStateAuthority)
            {
                var fireRatePause = 60 / _weaponConfig.FireRate;
                NetFireRatePause = TickTimer.CreateFromSeconds(Runner, fireRatePause);
            }

            _visibleFireCount = NetFireCount;
        }

        public override void Render()
        {
            if (_visibleFireCount < NetFireCount)
            {
                weaponView.ShotVfx();
                weaponView.ShotSfx();
                for (int i = _visibleFireCount; i < NetFireCount; i++)
                {
                    var data = NetProjectileData[i % NetProjectileData.Length];
                    _shooter.ShootView(ref data);
                }

                _visibleFireCount = NetFireCount;
            }
        }

        public void SetWeapon(WeaponId weaponId)
        {
            (_weaponConfig, _shooter) = _weaponFactory.Create(weaponId, GetComponent<IEntity>(), shootPoint);
            
            NetEquippedWeapon = weaponId;
            
            var fireRatePause = 60 / _weaponConfig.FireRate;
            NetFireRatePause = TickTimer.CreateFromSeconds(Runner, fireRatePause);
            
            Debug.Log($"{Object.InputAuthority} | {weaponId} | {NetEquippedWeapon}");
        }

        public void TryShoot()
        {
            if (NetFireRatePause.Expired(Runner))
            {
                var isHit = _shooter.Shoot(HasStateAuthority, out var data);
                var fireRatePause = 60 / _weaponConfig.FireRate;
                NetFireRatePause = TickTimer.CreateFromSeconds(Runner, fireRatePause);

                if (isHit)
                {
                    NetProjectileData.Set(NetFireCount % NetProjectileData.Length, data);
                    NetFireCount++;
                }
            }
        }
        
        public void OnDrawGizmos() 
            => _shooter?.OnDrawGizmos();

        private void OnNetEquippedWeaponChanged() 
            => SetWeapon(NetEquippedWeapon);
    }
}