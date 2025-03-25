using System;
using App.Armor;
using App.EventBus;
using App.Weapons;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public abstract class NetEntityBase : NetworkBehaviour, IEntity
    {
        [SerializeField] protected EntityConfig config;

        [Networked] [field: ReadOnly] public int NetHealthPoints { get; protected set; }
        [Networked] [OnChangedRender(nameof(ChangeArmor))] [field: ReadOnly] public int NetArmorLevel { get; protected set; }
        [Networked] [field: ReadOnly, SerializeField] protected Vector3 NetVelocity { get; set; }

        public GameObject GameObject => gameObject;
        public EntityIdentifier Identifier { get; } = new();
        public abstract EntityType EntityType { get; }

        protected float Gravity => config.Gravity;
        protected float WalkSpeed => config.WalkSpeed - _armor.WalkSpeedDecrease;
        protected float SprintSpeed => config.SprintSpeed - _armor.SprintSpeedDecrease;
        protected float MoveAcceleration => config.MoveAcceleration - _armor.MoveAccelerationDecrease;
        
        protected abstract IEventBus EventBus { get; set; }
        protected abstract ArmorsConfig ArmorsConfig { get; set; }
        protected NetWeapon NetWeapon;

        private ArmorConfig _armor;
        
        public event Action OnDeath;

        protected virtual void Awake()
        {
            NetWeapon = GetComponent<NetWeapon>();
        }

        public override void Spawned()
        {
            _armor = ArmorsConfig.GetArmor(0);
            NetHealthPoints = config != null ? config.InitialHealthPoints : 100;
            Debug.Log($"Spawned: [{Object.InputAuthority}]: [{NetWeapon.NetEquippedWeapon}]");
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

        public void SetArmor(int armorLevel) 
            => NetArmorLevel = armorLevel;

        public ArmorConfig GetArmor()
            => _armor;

        public abstract string GetName();

        private void ChangeArmor() 
            => _armor = ArmorsConfig.GetArmor(NetArmorLevel);
    }
}