using App.Health;

namespace App.Entities.Reviving.FSM.PlayerStates
{
    public class StartBleeding : PlayerReviveState
    {
        public StartBleeding(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView) 
            : base(netReviver, netHealth, reviveView) { }

        protected override void OnEnterState() 
            => NetReviver.BleedTimer = 10f;

        protected override void OnEnterStateRender()
        {
            if (ReviveView != null)
                ReviveView.ToggleVisibility(false);
        }

        protected override void OnFixedUpdate() 
            => TryActivateState<Bleeding>();
    }
}