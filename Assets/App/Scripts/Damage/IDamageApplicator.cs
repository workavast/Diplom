using App.Entities;
using App.PlayerEntities;
using Fusion;
using UnityEngine;

namespace App.Damage
{
    public interface IDamageApplicator
    {
        public bool FriendlyFire { get; }
        public float DamageScale { get; }

        public void TryApplyDamage(float damage, GameObject receiver, IEntity shooter);
    }
}