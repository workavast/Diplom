using App.Enemy;
using App.Entities;
using App.PlayerInput;
using App.Players.Nicknames;
using App.Weapons;
using Avastrad.EventBusFramework;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(PlayerView))]
    public class NetPlayerController : NetEntityBase
    {
        private PlayersRepository _playersRepository;
        private NicknamesProvider _nicknamesProvider;
        
        public PlayerView PlayerView { get; private set; }
        public PlayerRef PlayerRef => Object.InputAuthority;

        public override EntityType EntityType => EntityType.Player;

        [Inject]
        public void Construct(PlayersRepository playersRepository, NicknamesProvider nicknamesProvider, IEventBus eventBus)
        {
            _playersRepository = playersRepository;
            _nicknamesProvider = nicknamesProvider;
            base.Construct(eventBus);
        }

        protected override void Awake()
        {
            base.Awake();
            PlayerView = GetComponent<PlayerView>();
        }

        public override void Spawned()
        {
            base.Spawned();
            _playersRepository.Add(Object.InputAuthority, this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            _playersRepository.Remove(this);
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputData input))
            {
                var moveDirection = Vector3.right * input.HorizontalInput + Vector3.forward * input.VerticalInput;

                Vector3 lookPoint;
                var lookDirection = input.LookDirection;
                if (lookDirection == default || lookDirection == Vector2.zero)
                    lookPoint = PlayerView.transform.position + PlayerView.transform.forward;
                else
                    lookPoint = PlayerView.transform.position + lookDirection.X0Y();

                var moveSpeed = input.Buttons.IsSet(PlayerButtons.Sprint) ? config.SprintSpeed : config.WalkSpeed;
                PlayerView.Move(moveDirection, moveSpeed, config.Gravity, Runner.DeltaTime, config.SprintSpeed);
                PlayerView.SetLookPoint(lookPoint);

                if ((HasStateAuthority || HasInputAuthority) && input.Buttons.IsSet(PlayerButtons.Fire))
                {
                    NetWeapon = GetComponent<NetWeapon>();
                    NetWeapon.TryShoot();
                }
            }

#if UNITY_EDITOR
            if (HasStateAuthority && Input.GetKeyDown(KeyCode.Q)) 
                TakeDamage(999, this);
#endif
        }

        public override string GetName()
        {
            Debug.Log($"{HasStateAuthority} {HasInputAuthority} {Object == null}");
            
            return _nicknamesProvider.GetNickName(PlayerRef);
        }
    }
}

