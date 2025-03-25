using System;
using App.Armor;
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
            if (receiverEntity == null)
                return;

            var armorScale = receiverEntity.GetArmor().DamageScale;
            
            switch (receiverEntity.EntityType)
            {
                case EntityType.Default:
                    DamageDefault(damage * armorScale, receiverEntity, shooter);
                    break;
                case EntityType.Player:
                    DamagePlayer(damage * armorScale, receiverEntity, shooter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected abstract void DamagePlayer(float damage, IDamageable receiver, IEntity shooter);
        protected abstract void DamageDefault(float damage, IDamageable receiver, IEntity shooter);
    }
}