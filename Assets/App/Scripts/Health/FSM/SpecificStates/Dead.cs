namespace App.Health.FSM.SpecificStates
{
    public class Dead : HealthState
    {
        public Dead(NetHealth netEntity)
            : base(netEntity) { }
    }
}