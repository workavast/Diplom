using App.Entities.Player;
using App.NewDirectory1;
using Fusion;

namespace App.ExtractionZone.FSM.SpecificStates
{
    public class Countdown : ExtractionZoneState
    {
        public Countdown(NetExtractionZone netExtractionZone, ExtractionZoneConfig config, NetGameState netGameState,
            PlayersEntitiesRepository playersEntitiesRepository, ExtractionZoneView extractionZoneView) 
            : base(netExtractionZone, config, netGameState, playersEntitiesRepository, extractionZoneView) { }

        protected override void OnEnterState() 
            => NetExtractionZone.ExtractionTimer = TickTimer.CreateFromSeconds(Runner, ExtractionTime);

        protected override void OnEnterStateRender()
            => ExtractionZoneView.ToggleCountdownVisibility(true);

        protected override void OnExitState()
        {
            NetExtractionZone.ExtractionTimer = TickTimer.None;
        }

        protected override void OnFixedUpdate()
        {
            if (!NetGameState.GameIsRunning)
                return;
            
            if (!AllPlayersInZone())
            {
                TryActivateState<Idle>();
                return;
            }

            if (NetExtractionZone.ExtractionTimer.Expired(Runner))
            {
                TryActivateState<Extract>();
                return;
            }
        }
    }
}