using System;
using App.Armor;
using App.Damage;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public interface IEntity : IDamageable
    {
        bool IsActive { get; }
        EntityIdentifier Identifier { get; }
        EntityType EntityType { get; }
        GameObject GameObject { get; }
        Transform Transform => GameObject.transform;
        NetworkRunner Runner { get; }
        NetworkObject Object { get; }
        float NetHealthPoints { get; }
        int NetArmorLevel { get; }

        public event Action<IEntity> OnDeathEntity;

        ArmorConfig GetArmor();
        string GetName();
    }

    public static class EntitiesExtension
    {
        public static bool Is(this IEntity source, IEntity other)
        {
            if (source == null && other == null)
                return true;
            if (source == null || other == null)
                return false;

            return source.Identifier.Id == other.Identifier.Id;
        }
        
        public static bool IsAlive(this IEntity source) => source.IsActive && source.NetHealthPoints > 0;
        public static bool IsDead(this IEntity source) => !IsAlive(source);
    }
}