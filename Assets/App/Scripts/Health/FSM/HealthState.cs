using Fusion;
using Fusion.Addons.FSM;

namespace App.Health.FSM
{
    public abstract class HealthState : State<HealthState>
    {
        protected readonly NetHealth NetHealth;
        protected float HealthPoints => NetHealth.NetHealthPoints;

        protected NetworkRunner Runner => NetHealth.Runner;
        
        protected HealthState(NetHealth netHealth)
        {
            NetHealth = netHealth;
        }
    }
}