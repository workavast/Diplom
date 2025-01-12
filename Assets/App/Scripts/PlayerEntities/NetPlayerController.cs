using System;
using App.Damage;
using App.Entities;
using App.EventBus;
using App.PlayerInput;
using App.Weapons;
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

        [Networked, HideInInspector] public int HealthPoints { get; private set; }
        [Networked] private TickTimer AttackDelay { get; set; }
        
        public GameObject GameObject => gameObject;
        public EntityIdentifier Identifier { get; } = new();
        public EntityType EntityType => EntityType.Player;
        public PlayerRef PlayerRef => Object.InputAuthority;
        public PlayerView PlayerView { get; private set; }

        //Injected fields
        private PlayersRepository _playersRepository;
        private WeaponFactory _weaponFactory;
        private IEventBus _eventBus;
        
        private Weapon _weapon;
            
        public event Action OnDeath;

        [Inject]
        public void Construct(PlayersRepository playersRepository, WeaponFactory weaponFactory, IEventBus eventBus)
        {
            _playersRepository = playersRepository;
            _weaponFactory = weaponFactory;
            _eventBus = eventBus;
            
            SetWeapon(WeaponId.None);
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

        public void SetWeapon(WeaponId weaponId)
        {
            _weapon = _weaponFactory.Create(weaponId, this, shootPoint);
        }
        
        private void Shoot()
        {
            if (HasStateAuthority) 
                AttackDelay = TickTimer.CreateFromSeconds(Runner, config.AttackDaley);

            _weapon.Shoot(HasStateAuthority);
        }

        private void OnDrawGizmos()
        {
            _weapon.OnDrawGizmos();
        }
    }
}