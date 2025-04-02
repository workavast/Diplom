using App.Health;
using Fusion;
using Fusion.Addons.FSM;

namespace App.Entities.Reviving.FSM
{
    public abstract class ReviveState : State<ReviveState>
    {
        protected readonly NetReviver NetReviver;
        protected readonly NetHealth NetHealth;
        protected readonly ReviveView ReviveView;

        protected PlayerRef Owner => NetReviver.Object.InputAuthority;
        protected NetworkRunner Runner => NetReviver.Runner;
        
        protected ReviveState(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView)
        {
            NetReviver = netReviver;
            NetHealth = netHealth;
            ReviveView = reviveView;
        }
    }
}