using System;
using App.Armor;
using App.Health;
using App.Weapons;
using Avastrad.EventBusFramework;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public abstract class NetEntity : NetworkBehaviour, IEntity
    {
        [SerializeField] private EntityConfig config;
        [SerializeField] private SolderView solderView;
        [SerializeField] private NetHealth health;
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private NetworkCharacterController netCharacterController;

        [Networked] [field: ReadOnly, SerializeField] public Vector3 NetVelocity { get; private set; }
        [Networked] [OnChangedRender(nameof(ChangeArmor))] [field: ReadOnly] public int NetArmorLevel { get; private set; }

        public bool IsActive { get; private set; }
        public GameObject GameObject => gameObject;
        public EntityIdentifier Identifier { get; } = new();
        public abstract EntityType EntityType { get; }
        public bool RequiredReload => NetWeapon.RequiredReload;
        public float MaxHealthPoints => health.MaxHealthPoints;
        public float NetHealthPoints => health.NetHealthPoints;
        
        public int MaxAmmo => NetWeapon.MaxAmmo;
        public int CurrentAmmo => NetWeapon.CurrentAmmo;
        
        protected float Gravity => config.Gravity;
        protected float WalkSpeed => config.WalkSpeed - _armor.WalkSpeedDecrease;
        protected float SprintSpeed => config.SprintSpeed - _armor.SprintSpeedDecrease;
        protected float MoveAcceleration => config.MoveAcceleration - _armor.MoveAccelerationDecrease;
        
        protected abstract IEventBus EventBus { get; set; }
        protected abstract ArmorsConfig ArmorsConfig { get; set; }
        protected NetWeapon NetWeapon;

        private ArmorConfig _armor;

        public event Action OnKnockout;
        public event Action OnDeath;
        public event Action<IEntity> OnDeathEntity;
        public event Action OnWeaponShot; 

        protected virtual void Awake()
        {
            NetWeapon = GetComponent<NetWeapon>();

            health.OnKnockout += () => OnKnockout?.Invoke();
            health.OnDeath += () => OnDeath?.Invoke();
            health.OnDeathEntity += (_) => OnDeathEntity?.Invoke(this);
        }

        public override void Spawned()
        {
            IsActive = true;
            _armor = ArmorsConfig.GetArmor(0);
            hitbox.HitboxActive = 
                characterController.enabled = 
                netCharacterController.enabled = true;
            Debug.Log($"Spawned: [{Object.InputAuthority}]: [{NetWeapon.NetEquippedWeapon}]");
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            IsActive = false;
        }

        public override void FixedUpdateNetwork()
        {
            hitbox.HitboxActive = 
                characterController.enabled = 
                    netCharacterController.enabled = health.IsAlive;
        }

        public override void Render()
        {
            solderView.MoveView(NetVelocity, SprintSpeed);
        }

        public void TakeDamage(float damage, IEntity killer) 
            => health.TakeDamage(damage, killer);

        public void SetWeapon(WeaponId weaponId)
        {
            NetWeapon = GetComponent<NetWeapon>();
            NetWeapon.SetWeapon(weaponId);
        }

        public void SetArmor(int armorLevel) 
            => NetArmorLevel = armorLevel;

        public bool IsAlive() 
            => IsActive && health.IsAlive;

        public ArmorConfig GetArmor()
            => _armor;

        public abstract string GetName();

        public void TryShoot()
        {
            if (!health.IsAlive)
            {
                Debug.LogError($"You try shoot by entity that un full alive: [{gameObject.name}]");
                return;
            }
            
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
            if (!health.IsAlive)
            {
                Debug.LogError($"You try rotate entity that un full alive: [{gameObject.name}]");
                return;
            }
            
            Vector3 lookPoint;
            if (lookDirection == default || lookDirection == Vector2.zero)
                lookPoint = solderView.transform.position + solderView.transform.forward;
            else
                lookPoint = solderView.transform.position + lookDirection.X0Y();
                
            solderView.SetLookPoint(lookPoint);
        }
        
        public void CalculateVelocity(float horizontalInput, float verticalInput, bool isSprint)
        {
            if (!health.IsAlive)
            {
                Debug.LogError($"You try move entity that un full alive: [{gameObject.name}]");
                return;
            }
            
            if (!characterController.enabled)
            {
                Debug.LogWarning($"You try move entity that characterController un active: [{gameObject.name}]");
                return;
            }
            
            var targetVelocity = GetUnscaledVelocity(horizontalInput, verticalInput, isSprint);
            
            var currentVelocity = new Vector3(NetVelocity.x, 0, NetVelocity.z);
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, MoveAcceleration * Runner.DeltaTime);
            currentVelocity.y = 0;
            
            NetVelocity = new Vector3(currentVelocity.x, targetVelocity.y, currentVelocity.z);
            characterController.Move(NetVelocity * Runner.DeltaTime);
        }

        private Vector3 GetUnscaledVelocity(float horizontalInput, float verticalInput, bool isSprint)
        {
            var moveDirection = Vector3.right * horizontalInput + Vector3.forward * verticalInput;
            var moveSpeed = isSprint ? SprintSpeed : WalkSpeed;
           
            var unscaledGravityVelocity = Gravity * Vector3.up;
            var unscaledVelocity = moveSpeed * moveDirection;
            
            return  unscaledGravityVelocity + unscaledVelocity;
        }
        
        private void ChangeArmor() 
            => _armor = ArmorsConfig.GetArmor(NetArmorLevel);
    }
}