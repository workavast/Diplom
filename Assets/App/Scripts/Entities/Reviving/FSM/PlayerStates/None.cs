using App.Health;

namespace App.Entities.Reviving.FSM.PlayerStates
{
    public class None : PlayerReviveState
    {
        public None(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView) 
            : base(netReviver, netHealth, reviveView) { }

        protected override void OnFixedUpdate()
        {
            if (NetHealth.IsKnockout) 
                TryActivateState<StartBleeding>();
        }

        protected override void OnEnterStateRender() 
            => ReviveView.ToggleVisibility(false);
    }
}