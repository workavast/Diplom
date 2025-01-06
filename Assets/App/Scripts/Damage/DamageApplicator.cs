using System;
using App.Entities;
using UnityEngine;

namespace App.Damage
{
    public abstract class DamageApplicator : IDamageApplicator
    {
        public bool FriendlyFire { get; }
        public float DamageScale { get; }

        protected DamageApplicator(bool hasFriendlyFire)
        {
            FriendlyFire = hasFriendlyFire;
            DamageScale = 1;
        }

        public void TryApplyDamage(float damage, GameObject receiver, IEntity shooter)
        {
            var receiverEntity = receiver.GetComponent<IEntity>();
            var damageable = receiver.GetComponent<IDamageable>();
            
            if (receiverEntity == null || damageable == null)
                return;

            switch (receiverEntity.EntityType)
            {
                case EntityType.Default:
                    DamageDefault(damage, damageable, shooter);
                    break;
                case EntityType.Player:
                    DamagePlayer(damage, damageable, shooter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected abstract void DamagePlayer(float damage, IDamageable receiver, IEntity shooter);
        protected abstract void DamageDefault(float damage, IDamageable receiver, IEntity shooter);
    }
}