using Fusion.Addons.FSM;

namespace App.ExtractionZone.FSM
{
    public class ExtractionZoneStateMachine : StateMachine<ExtractionZoneState>
    {
        public ExtractionZoneStateMachine(string name, params ExtractionZoneState[] states)
            : base(name, states) { }
    }
}