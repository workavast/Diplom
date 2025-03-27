using System;
using App.Armor;
using App.PlayerInput;
using App.Players.Nicknames;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Entities.Player
{
    public class NetPlayerEntity : NetEntityBase
    {
        public PlayerRef PlayerRef => Object.InputAuthority;
        public override EntityType EntityType => EntityType.Player;
        
        protected override IEventBus EventBus { get; set; }
        protected override ArmorsConfig ArmorsConfig { get; set; }

        private PlayersEntitiesRepository _playersEntitiesRepository;
        private NicknamesProvider _nicknamesProvider;
        
        public event Action OnWeaponShot; 
        
        [Inject]
        public void Construct(PlayersEntitiesRepository playersEntitiesRepository, NicknamesProvider nicknamesProvider, 
            IEventBus eventBus, ArmorsConfig armorsConfig)
        {
            _playersEntitiesRepository = playersEntitiesRepository;
            _nicknamesProvider = nicknamesProvider;
            EventBus = eventBus;
            ArmorsConfig = armorsConfig;
        }

        public override void Spawned()
        {
            base.Spawned();
            _playersEntitiesRepository.Add(Object.InputAuthority, this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            _playersEntitiesRepository.Remove(this);
        }

        public override void FixedUpdateNetwork()
        {
            var hasInput = GetInput(out PlayerInputData input);
            if (hasInput)
            {
                RotateByLookDirection(input.LookDirection);

                var isSprint = input.Buttons.IsSet(PlayerButtons.Sprint);
                CalculateVelocity(input.HorizontalInput, input.VerticalInput, isSprint);
                
                if ((HasStateAuthority || HasInputAuthority) && input.Buttons.IsSet(PlayerButtons.Fire) && NetWeapon.CanShot)
                {
                    if (NetWeapon.TryShoot())
                        OnWeaponShot?.Invoke();
                }

                var reloadRequest = input.Buttons.IsSet(PlayerButtons.Reload) || NetWeapon.RequiredReload;
                if ((HasStateAuthority || HasInputAuthority) && reloadRequest)
                {
                    if (NetWeapon.CanReload) 
                        NetWeapon.TryReload();
                }
            }
            
#if UNITY_EDITOR
            if (HasStateAuthority && Input.GetKeyDown(KeyCode.Q)) 
                TakeDamage(999, this);
#endif
        }

        public override string GetName() 
            => _nicknamesProvider.GetNickName(PlayerRef);
    }
}

