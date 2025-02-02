using App.Enemy;
using App.Entities;
using App.PlayerInput;
using App.Weapons;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(PlayerView))]
    public class NetPlayerController : NetEntityBase
    {
        private PlayersRepository _playersRepository;
        public PlayerView PlayerView { get; private set; }
        public PlayerRef PlayerRef => Object.InputAuthority;

        public override EntityType EntityType => EntityType.Player;

        [Inject]
        public void Construct(PlayersRepository playersRepository, IEventBus eventBus)
        {
            _playersRepository = playersRepository;
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
                var lookPoint = new Vector3(input.LookPoint.x, PlayerView.transform.position.y, input.LookPoint.y);
                PlayerView.Move(moveDirection, config.MoveSpeed, config.Gravity, Runner.DeltaTime);
                PlayerView.SetLookPoint(lookPoint);

                if ((HasStateAuthority || HasInputAuthority) && input.Buttons.IsSet(PlayerButtons.Fire))
                {
                    NetWeapon = GetComponent<NetWeapon>();
                    NetWeapon.TryShoot();
                }
            }
        }

        public override string GetName() 
            => "PLAYER";
    }
}

