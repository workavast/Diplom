using Fusion;
using Fusion.Addons.FSM;

namespace App.Entities.Player.Controlling.FSM
{
    public abstract class PlayerState : State
    {
        protected readonly NetPlayerController NetController;
        protected readonly NetEntity NetEntity;

        protected PlayerRef Owner => NetController.Object.InputAuthority;
        protected NetworkBehaviour OwnerBehaviour => NetEntity;
        protected NetworkRunner Runner => NetController.Runner;

        protected float HealthPoints => NetEntity.NetHealthPoints;
        protected bool HasStateAuthority => OwnerBehaviour.HasStateAuthority; 
        protected bool HasInputAuthority => OwnerBehaviour.HasInputAuthority;
        
        protected PlayerState(NetPlayerController netController, NetEntity netEntity)
        {
            NetController = netController;
            NetEntity = netEntity;
        }
    }
}