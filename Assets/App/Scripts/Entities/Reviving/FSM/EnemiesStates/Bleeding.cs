using App.Health;
using UnityEngine;

namespace App.Entities.Reviving.FSM.EnemiesStates
{
    public class Bleeding : EnemyReviveState
    {
        public Bleeding(NetReviver netReviver, NetHealth netHealth) 
            : base(netReviver, netHealth) { }
        
        protected override void OnFixedUpdate()
        {
            var bleedTimer = NetReviver.BleedTimer - Runner.DeltaTime;
            NetReviver.BleedTimer = Mathf.Clamp(bleedTimer, 0, float.PositiveInfinity);

            if (NetReviver.BleedTimer <= 0) 
                NetHealth.PermanentDeath();

            if (NetHealth.IsAlive || NetHealth.IsDead) 
                TryActivateState<None>();
        }
    }
}