using System;
using App.Damage;
using App.Entities;
using App.EventBus;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Enemy
{
    [RequireComponent(typeof(EnemyView))]
    public class NetEnemy : NetworkBehaviour, IEntity, IDamageable
    {
        public EntityIdentifier Identifier { get; } = new();
        
        [Networked] public int HealthPoints { get; private set; }
        [Inject] private IEventBus _eventBus;
        [Inject] private EnemiesRepository _enemiesRepository;

        public Action OnDeath;
        
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
        {
            return "throw new NotImplementedException();";
        }
    }
}