using App.Health;

namespace App.Entities.Reviving.FSM
{
    public abstract class PlayerReviveState : ReviveState
    {
        protected readonly ReviveView ReviveView;

        protected PlayerReviveState(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView) 
            : base(netReviver, netHealth)
        {
            ReviveView = reviveView;
        }
    }
}