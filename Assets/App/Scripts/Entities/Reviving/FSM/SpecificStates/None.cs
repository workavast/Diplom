using App.Health;

namespace App.Entities.Reviving.FSM.SpecificStates
{
    public class None : ReviveState
    {
        public None(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView) 
            : base(netReviver, netHealth, reviveView) { }

        protected override void OnFixedUpdate()
        {
            if (NetHealth.IsKnockout) 
                NetReviver.TryActivateState<WaitRevive>();
        }
        
        protected override void OnRender()
        {
            ReviveView.ToggleVisibility(false);
        }
    }
}