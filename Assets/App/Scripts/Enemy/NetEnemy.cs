using System;
using App.Damage;
using Fusion;
using UnityEngine;

namespace App.Enemy
{
    [RequireComponent(typeof(EnemyView))]
    public class NetEnemy : NetworkBehaviour, IDamageable
    {
        [Networked] private int HealthPoints { get; set; }

        public Action OnDeath;
        
        public override void Spawned()
        {
            HealthPoints = 100;
        }
        
        public void TakeDamage(int damage, PlayerRef shooter)
        {
            HealthPoints -= damage;
            
            Debug.Log($"HealthPoints: {HealthPoints}");
            
            if (HasStateAuthority)
            {
                if (HealthPoints <= 0)
                {
                    Runner.Despawn(Object);
                    OnDeath?.Invoke();
                    OnDeath = null;
                    Debug.Log($"Enemy is death");
                }
            }
        }
    }
}