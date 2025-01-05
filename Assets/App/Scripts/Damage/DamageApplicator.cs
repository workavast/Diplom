using App.Enemy;
using App.Entities;
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

        public void TryApplyDamage(float damage, GameObject receiver, IEntity shooter)
        {
            TryApplyDamageByPlayer(damage, receiver, shooter);
        }
        
        public void TryApplyDamage(float damage, NetPlayerController receiver, IEntity shooter)
        {
            if (FriendlyFire)
                receiver.TakeDamage(damage, shooter);
        }
        
        public void TryApplyDamage(float damage, NetEnemy receiver, IEntity shooter)
        {
            receiver.TakeDamage(damage, shooter);
        }
        
        public void ApplyDamage(float damage, IDamageable damageable, IEntity shooter)
        {
            damageable.TakeDamage(damage, shooter);
        }

        private void TryApplyDamageByPlayer(float damage, GameObject receiver, IEntity shooter)
        {
            damage *= PlayerDamageScale;
            
            var player = receiver.GetComponent<NetPlayerController>();
            if (player != null)
                TryApplyDamage(damage, player, shooter);

            var enemy = receiver.GetComponent<NetEnemy>();
            if (enemy != null)
                TryApplyDamage(damage, enemy, shooter);
        }
        
        // private void TryApplyDamageByEnemy(float damage, GameObject receiver)
        // {
        //     damage *= EnemyDamageScale;
        //     
        //     var player = receiver.GetComponent<NetPlayerController>();
        //     if (player != null)
        //         TryApplyDamage(damage, player);
        //
        //     var enemy = receiver.GetComponent<NetEnemy>();
        //     if (enemy != null)
        //         TryApplyDamage(damage, enemy);
        // }
    }
}