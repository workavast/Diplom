using System;
using App.Armor;
using App.Damage;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public interface IEntity : IDamageable
    {
        EntityIdentifier Identifier { get; }
        EntityType EntityType { get; }
        GameObject GameObject { get; }
        Transform Transform => GameObject.transform;
        NetworkRunner Runner { get; }
        NetworkObject Object { get; }
        int NetHealthPoints { get; }
        int NetArmorLevel { get; }

        ArmorConfig GetArmor();
        string GetName();
    }

    public static class EntitiesEqualer
    {
        public static bool Is(this IEntity source, IEntity other)
        {
            if (source == null && other == null)
                return true;
            if (source == null || other == null)
                return false;

            return source.Identifier.Id == other.Identifier.Id;
        }
    }
}