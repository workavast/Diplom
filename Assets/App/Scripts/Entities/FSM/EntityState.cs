using Fusion;
using Fusion.Addons.FSM;

namespace App.Entities.FSM
{
    public abstract class EntityState : State
    {
        protected readonly NetEntity NetEntity;

        protected PlayerRef Owner => NetEntity.Object.InputAuthority;
        protected NetworkBehaviour OwnerBehaviour => NetEntity;
        protected NetworkRunner Runner => NetEntity.Runner;

        protected float HealthPoints => NetEntity.NetHealthPoints;
        protected bool HasStateAuthority => OwnerBehaviour.HasStateAuthority; 
        protected bool HasInputAuthority => OwnerBehaviour.HasInputAuthority;
        
        protected EntityState(NetEntity netEntity)
        {
            NetEntity = netEntity;
        }
    }
}