using System;
using App.Damage;
using App.Entities;
using App.EventBus;
using App.PlayerEntities.Shooting;
using App.PlayerInput;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(PlayerView))]
    public class NetPlayerController : NetworkBehaviour, IEntity, IDamageable
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private PlayerEntityConfig config;

        public GameObject GameObject => gameObject;
        [Networked, HideInInspector] public int HealthPoints { get; private set; }
        [Networked] private TickTimer AttackDelay { get; set; }
        
        public EntityIdentifier Identifier { get; } = new();
        public EntityType EntityType => EntityType.Player;
        public PlayerRef PlayerRef => Object.InputAuthority;
        public PlayerView PlayerView { get; private set; }

        private Shooter _shooter;
        private PlayersRepository _playersRepository;
        private IEventBus _eventBus;

        public event Action OnDeath;

        [Inject]
        public void Construct(PlayersRepository playersRepository, IEventBus eventBus, ShooterFactory shooterFactory)
        {
            _playersRepository = playersRepository;
            _eventBus = eventBus;
            _shooter = shooterFactory.CreateShoot(this, shootPoint, config);
        }
        
        private void Awake()
        {
            PlayerView = GetComponent<PlayerView>();
        }

        public override void Spawned()
        {
            _playersRepository.Add(Object.InputAuthority, this);
            HealthPoints = config.InitialHealthPoints;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
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
                
                if (input.Buttons.IsSet(PlayerButtons.Fire) && AttackDelay.ExpiredOrNotRunning(Runner)) 
                    Shoot();
            }
        }

        public string GetName()
        {
            return "PLAYER";
        }
        
        public void TakeDamage(float damage, IEntity shooter)//shooter need to give him points
        {
            HealthPoints -= (int)damage;
            if (HasStateAuthority)
            {
                if (HealthPoints <= 0)
                {
                    _eventBus.Invoke(new OnKill(Identifier.Id, shooter.Identifier.Id));
                    OnDeath?.Invoke();
                }
            }
        }
        
        private void Shoot()
        {
            if (HasStateAuthority) 
                AttackDelay = TickTimer.CreateFromSeconds(Runner, config.AttackDaley);

            _shooter.Shoot(HasStateAuthority);
        }

        private void OnDrawGizmos()
        {
            _shooter.OnDrawGizmos();
        }
    }
}