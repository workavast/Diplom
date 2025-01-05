using App.Enemy;
using App.PlayerEntities;
using Fusion;
using UnityEngine;

namespace App.Damage
{
    public interface IDamageApplicator
    {
        public bool FriendlyFire { get; }
        public float PlayerDamageScale { get; }
        public float EnemyDamageScale { get; }

        public void TryApplyDamage(int damage, GameObject receiver, PlayerRef shooter);
        public void TryApplyDamage(int damage, NetPlayerController receiver, PlayerRef shooter);
        public void TryApplyDamage(int damage, NetEnemy receiver, PlayerRef shooter);

        public void ApplyDamage(int damage, IDamageable damageable, PlayerRef shooter);
    }
}