using System;
using App.Armor;
using App.EventBus;
using App.Weapons;
using Avastrad.EventBusFramework;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public abstract class NetEntity : NetworkBehaviour, IEntity
    {
        [SerializeField] protected EntityConfig config;
        [SerializeField, Tooltip("Can be null")] protected SolderView solderView;
        [SerializeField, Tooltip("Can be null")] private CharacterController characterController;

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
        public event Action OnWeaponShot; 

        protected virtual void Awake()
        {
            NetWeapon = GetComponent<NetWeapon>();
            
            if (solderView == null)
                solderView = ComponentExt.GetComponent<SolderView>(this);
            
            if (characterController == null)
                characterController = ComponentExt.GetComponent<CharacterController>(this);
        }

        public override void Spawned()
        {
            _armor = ArmorsConfig.GetArmor(0);
            NetHealthPoints = config != null ? config.InitialHealthPoints : 100;
            Debug.Log($"Spawned: [{Object.InputAuthority}]: [{NetWeapon.NetEquippedWeapon}]");
        }

        public override void Render()
        {
            solderView.MoveView(NetVelocity, SprintSpeed);
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

        public void TryShoot()
        {
            if (NetWeapon.CanShot && NetWeapon.TryShoot())
                OnWeaponShot?.Invoke();
        }
        
        public void TryReload()
        {
            if (NetWeapon.CanReload) 
                NetWeapon.TryReload();
        }
        
        public void RotateByLookDirection(Vector2 lookDirection)
        {
            Vector3 lookPoint;
            if (lookDirection == default || lookDirection == Vector2.zero)
                lookPoint = solderView.transform.position + solderView.transform.forward;
            else
                lookPoint = solderView.transform.position + lookDirection.X0Y();
                
            solderView.SetLookPoint(lookPoint);
        }
        
        protected Vector3 GetUnscaledVelocity(float horizontalInput, float verticalInput, bool isSprint)
        {
            var moveDirection = Vector3.right * horizontalInput + Vector3.forward * verticalInput;
            var moveSpeed = isSprint ? SprintSpeed : WalkSpeed;
           
            var unscaledGravityVelocity = Gravity * Vector3.up;
            var unscaledVelocity = moveSpeed * moveDirection;
            
            return  unscaledGravityVelocity + unscaledVelocity;
        }
        
        public void CalculateVelocity(float horizontalInput, float verticalInput, bool isSprint)
        {
            var targetVelocity = GetUnscaledVelocity(horizontalInput, verticalInput, isSprint);
            
            var currentVelocity = new Vector3(NetVelocity.x, 0, NetVelocity.z);
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, MoveAcceleration * Runner.DeltaTime);
            currentVelocity.y = 0;
            
            NetVelocity = new Vector3(currentVelocity.x, targetVelocity.y, currentVelocity.z);
            characterController.Move(NetVelocity * Runner.DeltaTime);
        }
    }
}