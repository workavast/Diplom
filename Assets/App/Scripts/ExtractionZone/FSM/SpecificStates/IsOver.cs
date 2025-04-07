using App.Entities.Player;
using App.NewDirectory1;

namespace App.ExtractionZone.FSM.SpecificStates
{
    public class IsOver : ExtractionZoneState
    {
        public IsOver(NetExtractionZone netExtractionZone, ExtractionZoneConfig config, NetGameState netGameState,
            PlayersEntitiesRepository playersEntitiesRepository, ExtractionZoneView extractionZoneView) 
            : base(netExtractionZone, config, netGameState, playersEntitiesRepository, extractionZoneView) { }

        protected override void OnEnterStateRender()
            => ExtractionZoneView.ToggleCountdownVisibility(true);
    }
}