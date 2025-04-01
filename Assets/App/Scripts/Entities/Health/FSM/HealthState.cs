using Fusion.Addons.FSM;

namespace App.Entities.Health.FSM
{
    public abstract class HealthState : State
    {
        protected readonly NetHealth NetHealth;
        protected float HealthPoints => NetHealth.NetHealthPoints;

        protected HealthState(NetHealth netHealth)
        {
            NetHealth = netHealth;
        }
    }
}