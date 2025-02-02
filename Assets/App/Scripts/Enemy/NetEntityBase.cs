using System;
using App.Damage;
using App.Entities;
using App.EventBus;
using App.PlayerEntities;
using App.Weapons;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Enemy
{
    public abstract class NetEntityBase : NetworkBehaviour, IEntity, IDamageable
    {
        [SerializeField] protected PlayerEntityConfig config;

        [Networked] public int NetHealthPoints { get; protected set; }

        public GameObject GameObject => gameObject;
        public EntityIdentifier Identifier { get; } = new();
        public abstract EntityType EntityType { get; }

        protected IEventBus EventBus;
        protected NetWeapon NetWeapon;
        
        public event Action OnDeath;
        
        [Inject]
        public virtual void Construct(IEventBus eventBus)
        {
            EventBus = eventBus;
        }

        protected virtual void Awake()
        {
            Debug.Log("Awake");
            NetWeapon = GetComponent<NetWeapon>();
        }

        public override void Spawned()
        {
            Debug.Log("Spawned");
            NetHealthPoints = config != null ? config.InitialHealthPoints : 100;
            Debug.Log($"Spawned: {Object.InputAuthority}: {NetWeapon.NetEquippedWeapon}");
        }
        
        public void TakeDamage(float damage, IEntity shooter)
        {
            NetHealthPoints -= (int)damage;

            if (HasStateAuthority && NetHealthPoints <= 0)
            {
                EventBus.Invoke(new OnKill(Identifier.Id, shooter.Identifier.Id));
                OnDeath?.Invoke();
                OnDeath = null;
                Debug.Log($"{GetName()} is dead");
                Runner.Despawn(Object);
            }
        }
        
        public void SetWeapon(WeaponId weaponId)
        {
            NetWeapon = GetComponent<NetWeapon>();
            NetWeapon.SetWeapon(weaponId);
        }
        
        public abstract string GetName();
    }
}