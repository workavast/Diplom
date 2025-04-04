using System;
using App.Armor;
using App.Health;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public class EntityBody : NetworkBehaviour, IEntity
    {
        [SerializeField] private NetHealth netHealth;
        [SerializeField] private NetEntity netEntity;
        
        public bool IsActive => netEntity.IsActive;
        public EntityIdentifier Identifier => netEntity.Identifier;
        public EntityType EntityType => netEntity.EntityType;
        public GameObject GameObject => netEntity.GameObject;
        public new NetworkRunner Runner => netEntity.Runner;
        public new NetworkObject Object => netEntity.Object;
        public float NetHealthPoints => netEntity.NetHealthPoints;
        public int NetArmorLevel => netEntity.NetArmorLevel;
        public event Action<IEntity> OnDeathEntity;

        public bool IsAlive() => netEntity.IsAlive();

        public ArmorConfig GetArmor() => netEntity.GetArmor();
        
        public string GetName() => netEntity.GetName();

        public void TakeDamage(float damage, IEntity killer) => netEntity.TakeDamage(damage, killer);

        private void Awake()
        {
            netEntity.OnDeathEntity += (entity) => OnDeathEntity?.Invoke(entity);
        }
    }
}