using App.Health;

namespace App.Entities.Reviving.FSM.EnemiesStates
{
    public class None : EnemyReviveState
    {
        public None(NetReviver netReviver, NetHealth netHealth) 
            : base(netReviver, netHealth) { }
        
        protected override void OnFixedUpdate()
        {
            if (NetHealth.IsKnockout) 
                TryActivateState<StartBleeding>();
        }
    }
}