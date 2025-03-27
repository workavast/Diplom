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
}