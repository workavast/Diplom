using System;
using App.Armor;
using App.PlayerInput;
using App.Players.Nicknames;
using Avastrad.EventBusFramework;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Entities.Player
{
    public class NetPlayerController : NetEntityBase
    {
        [SerializeField, Tooltip("Can be null")] private PlayerView playerView;
        [SerializeField, Tooltip("Can be null")] private CharacterController characterController;
        
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
        
        protected override void Awake()
        {
            base.Awake();

            if (playerView == null)
                playerView = ComponentExt.GetComponent<PlayerView>(this);

            if (characterController == null)
                characterController = ComponentExt.GetComponent<CharacterController>(this);
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
                CalculateVelocity(input);
                
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

        public override void Render()
        {
            playerView.MoveView(NetVelocity, SprintSpeed);
        }

        public override string GetName()
        {
            return _nicknamesProvider.GetNickName(PlayerRef);
        }

        private void CalculateVelocity(PlayerInputData input)
        {
            var targetVelocity = GetUnscaledVelocity(input);
            
            var currentVelocity = new Vector3(NetVelocity.x, 0, NetVelocity.z);
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, MoveAcceleration * Runner.DeltaTime);
            currentVelocity.y = 0;
            
            NetVelocity = new Vector3(currentVelocity.x, targetVelocity.y, currentVelocity.z);
            characterController.Move(NetVelocity * Runner.DeltaTime);
        }

        private void RotateByLookDirection(Vector2 lookDirection)
        {
            Vector3 lookPoint;
            if (lookDirection == default || lookDirection == Vector2.zero)
                lookPoint = playerView.transform.position + playerView.transform.forward;
            else
                lookPoint = playerView.transform.position + lookDirection.X0Y();
                
            playerView.SetLookPoint(lookPoint);
        }

        private Vector3 GetUnscaledVelocity(PlayerInputData input)
        {
            var moveDirection = Vector3.right * input.HorizontalInput + Vector3.forward * input.VerticalInput;
            var moveSpeed = input.Buttons.IsSet(PlayerButtons.Sprint) ? SprintSpeed : WalkSpeed;
           
            var unscaledGravityVelocity = Gravity * Vector3.up;
            var unscaledVelocity = moveSpeed * moveDirection;
            
            return  unscaledGravityVelocity + unscaledVelocity;
        }
    }
}

