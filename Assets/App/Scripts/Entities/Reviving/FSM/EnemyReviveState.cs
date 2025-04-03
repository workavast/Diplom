using App.Health;

namespace App.Entities.Reviving.FSM
{
    public abstract class EnemyReviveState : ReviveState
    {
        protected EnemyReviveState(NetReviver netReviver, NetHealth netHealth) 
            : base(netReviver, netHealth) { }
    }
}