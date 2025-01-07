using System;
using App.Damage;
using App.Entities;
using App.EventBus;
using App.PlayerEntities;
using App.Weapons;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Enemy
{
    [RequireComponent(typeof(EnemyView))]
    public class NetEnemy : NetworkBehaviour, IEntity, IDamageable
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private PlayerEntityConfig config;
        
        [Networked] public int HealthPoints { get; private set; }
        
        public EntityIdentifier Identifier { get; } = new();
        public EntityType EntityType => EntityType.Default;
        public GameObject GameObject => gameObject;

        private Weapon _weapon;
        private IEventBus _eventBus;
        private EnemiesRepository _enemiesRepository;
        
        public Action OnDeath;
        
        [Inject]
        public void Construct(EnemiesRepository enemiesRepository, IEventBus eventBus, WeaponFactory weaponFactory)
        {
            _enemiesRepository = enemiesRepository;
            _eventBus = eventBus;
            _weapon = weaponFactory.Create(WeaponId.Pistol, this, shootPoint);
        }
        
        public override void Spawned()
        {
            HealthPoints = 100;
            _enemiesRepository.Add(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _enemiesRepository.Remove(this);
        }

        public void TakeDamage(float damage, IEntity shooter)
        {
            HealthPoints -= (int)damage;
            
            Debug.Log($"HealthPoints: {HealthPoints}");
            
            if (HasStateAuthority)
            {
                if (HealthPoints <= 0)
                {
                    _eventBus.Invoke(new OnKill(Identifier.Id, shooter.Identifier.Id));
                    Runner.Despawn(Object);
                    OnDeath?.Invoke();
                    OnDeath = null;
                    Debug.Log($"Enemy is death");
                }
            }
        }
        
        public string GetName() 
            => nameof(NetEnemy);
        
        private void OnDrawGizmos()
        {
            _weapon.OnDrawGizmos();
        }
    }
}