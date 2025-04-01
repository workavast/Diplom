using System;
using App.Armor;
using App.Damage;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public class EntityProvider : MonoBehaviour, IEntity, IDamageable
    {
        [SerializeField] private NetEntity netEntity;

        public bool IsActive => netEntity.IsActive;
        public EntityIdentifier Identifier => netEntity.Identifier;
        public EntityType EntityType => netEntity.EntityType;
        public GameObject GameObject => netEntity.GameObject;
        public NetworkRunner Runner => netEntity.Runner;
        public NetworkObject Object => netEntity.Object;
        public float NetHealthPoints => netEntity.NetHealthPoints;
        public int NetArmorLevel => netEntity.NetArmorLevel;
        public event Action<IEntity> OnDeathEntity;

        public ArmorConfig GetArmor() => netEntity.GetArmor();
        
        public string GetName() => netEntity.GetName();

        public void TakeDamage(float damage, IEntity shooter) => netEntity.TakeDamage(damage, shooter);

        private void Awake()
        {
            netEntity.OnDeathEntity += (entity) => OnDeathEntity?.Invoke(entity);
        }
    }
}