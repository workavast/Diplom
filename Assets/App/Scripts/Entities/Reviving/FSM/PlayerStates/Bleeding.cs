using App.Entities.Player;
using App.Health;
using UnityEngine;

namespace App.Entities.Reviving.FSM.PlayerStates
{
    public class Bleeding : PlayerReviveState
    {
        private readonly BleedingView _bleedingView;
        private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        private readonly ReviveConfig _config;

        public Bleeding(NetReviver netReviver, NetHealth netHealth, ReviveView reviveView, BleedingView bleedingView,
            PlayersEntitiesRepository playersEntitiesRepository, ReviveConfig config) 
            : base(netReviver, netHealth, reviveView)
        {
            _bleedingView = bleedingView;
            _playersEntitiesRepository = playersEntitiesRepository;
            _config = config;
        }
        
        protected override void OnEnterStateRender()
        {
            if (_bleedingView != null)
                _bleedingView.ToggleVisibility(true);
            
            if (ReviveView != null)
                ReviveView.ToggleVisibility(false);
        }


        protected override void OnExitStateRender()
        {
            if (_bleedingView != null)
                _bleedingView.ToggleVisibility(false);
        }


        protected override void OnFixedUpdate()
        {
            var bleedTimer = NetReviver.BleedTimer - Runner.DeltaTime;
            NetReviver.BleedTimer = Mathf.Clamp(bleedTimer, 0, float.PositiveInfinity);

            if (NetReviver.BleedTimer <= 0) 
                NetHealth.PermanentDeath();

            if (NetHealth.IsAlive || NetHealth.IsDead)
            {
                TryActivateState<None>();
                return;
            }
            
            if (_playersEntitiesRepository.TryGetNearestPlayer(NetReviver.transform.position, _config.ReviveDistance, out _))
                TryActivateState<ReviveProcess>();
        }
        
        protected override void OnRender()
        {
            _bleedingView.SetValue(NetReviver.BleedTimer / 10f);
        }
    }
}