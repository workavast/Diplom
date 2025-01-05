using App.Enemy;
using App.PlayerEntities;
using Fusion;
using UnityEngine;

namespace App.Damage
{
    public class DamageApplicator : IDamageApplicator
    {
        public bool FriendlyFire { get; }
        public float PlayerDamageScale { get; }
        public float EnemyDamageScale { get; }

        public DamageApplicator(bool hasFriendlyFire)
        {
            FriendlyFire = hasFriendlyFire;
            PlayerDamageScale = EnemyDamageScale = 1;
        }

        public void TryApplyDamage(int damage, GameObject receiver, PlayerRef shooter)
        {
            var player = receiver.GetComponent<NetPlayerController>();
            if (player != null)
                TryApplyDamage(damage, player, shooter);

            var enemy = receiver.GetComponent<NetEnemy>();
            if (enemy != null)
                TryApplyDamage(damage, enemy, shooter);
        }
        
        public void TryApplyDamage(int damage, NetPlayerController receiver, PlayerRef shooter)
        {
            if (FriendlyFire)
                receiver.TakeDamage(damage, shooter);
        }
        
        public void TryApplyDamage(int damage, NetEnemy receiver, PlayerRef shooter)
        {
            receiver.TakeDamage(damage, shooter);
        }
        
        public void ApplyDamage(int damage, IDamageable damageable, PlayerRef shooter)
        {
            damageable.TakeDamage(damage, shooter);
        }
    }
}