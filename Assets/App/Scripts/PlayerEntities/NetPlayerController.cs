using System;
using App.Bullets;
using App.EventBus;
using App.PlayerInput;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(PlayerView))]
    public class NetPlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerEntityConfig config;
        
        [Networked, HideInInspector] private int HealthPoints { get; set; }
        [Networked, HideInInspector] private TickTimer AttackDelay { get; set; }

        public PlayerRef PlayerRef => Object.InputAuthority;

        private PlayersRepository _playersRepository;
        private NetBulletsSpawner _netBulletsSpawner;
        private PlayerView _playerView;
        private IEventBus _eventBus;

        public event Action OnDeath;

        [Inject]
        public void Construct(PlayersRepository playersRepository)
        {
            _playersRepository = playersRepository;
        }
        
        public void Initialize(NetBulletsSpawner netBulletsSpawner, IEventBus eventBus)
        {
            _netBulletsSpawner = netBulletsSpawner;
            _eventBus = eventBus;
        }
        
        private void Awake()
        {
            _playerView = GetComponent<PlayerView>();
        }

        public override void Spawned()
        {
            _playersRepository.Add(Object.InputAuthority, _playerView);
            HealthPoints = config.InitialHealthPoints;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersRepository.Remove(Object.InputAuthority);
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputData input))
            {
                var moveDirection = Vector3.right * input.HorizontalInput + Vector3.forward * input.VerticalInput;
                var lookPoint = new Vector3(input.LookPoint.x, _playerView.transform.position.y, input.LookPoint.y);
                _playerView.Move(moveDirection, Runner.DeltaTime);
                _playerView.SetLookPoint(lookPoint);
                
                if (!HasStateAuthority)
                    return;
                
                if (input.Buttons.IsSet(PlayerButtons.Fire) && AttackDelay.ExpiredOrNotRunning(Runner))
                {
                    AttackDelay = TickTimer.CreateFromSeconds(Runner, config.AttackDaley);
                    _netBulletsSpawner.Spawn(_playerView.transform.position, _playerView.transform.forward, PlayerRef);
                }
            }
        }

        public void TakeDamage(int damage, PlayerRef shooter)//shooter need to give him points
        {
            HealthPoints -= damage;
            if (HasStateAuthority)
            {
                if (HealthPoints <= 0)
                {
                    _eventBus.Invoke(new OnPlayerKill(Object.InputAuthority, shooter));
                    OnDeath?.Invoke();
                }
            }
        }
    }
}