using System;
using System.Collections.Generic;
using App.Entities.Player;
using App.ExtractionZone.FSM;
using App.ExtractionZone.FSM.SpecificStates;
using App.NewDirectory1;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;
using Zenject;

namespace App.ExtractionZone
{
    public class NetExtractionZone : NetworkBehaviour, IStateMachineOwner
    {
        [SerializeField] private NetGameState netGameState;
        [SerializeField] private ExtractionZoneConfig config;
        [SerializeField] private ExtractionZoneView extractionZoneView;

        [Networked] public TickTimer ExtractionTimer { get; set; }

        [Inject] private PlayersEntitiesRepository _playersEntitiesRepository;

        public bool IsSpawned { get; private set; }

        private int _lastRemainingTime;
        private TimeSpan _lastTimeSPan;

        private Idle _idle;
        private Countdown _countdown;
        private Extract _extract;
        private IsOver _isOver;
        private ExtractionZoneStateMachine _fsm;
        
        public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _idle = new Idle(this, config, netGameState, _playersEntitiesRepository, extractionZoneView);
            _countdown = new Countdown(this, config, netGameState, _playersEntitiesRepository, extractionZoneView);
            _extract = new Extract(this, config, netGameState, _playersEntitiesRepository, extractionZoneView);
            _isOver = new IsOver(this, config, netGameState, _playersEntitiesRepository, extractionZoneView);

            _fsm = new ExtractionZoneStateMachine("ExtractionZone", _idle, _countdown, _extract, _isOver);
            
            stateMachines.Add(_fsm);
        }

        public override void Spawned()
        {
            extractionZoneView.SetSize(config.ExtractionRadius);
            IsSpawned = true;
        }

        public override void Despawned(NetworkRunner runner, bool hasState) 
            => IsSpawned = false;

        public TimeSpan GetTime()
        {
            if (!IsSpawned || ExtractionTimer.ExpiredOrNotRunning(Runner))
                return TimeSpan.Zero;

            var remainingTime = (int)Math.Floor(ExtractionTimer.RemainingTime(Runner).Value);
            if (_lastRemainingTime == remainingTime)
                return _lastTimeSPan;
            
            _lastRemainingTime = remainingTime;
            
            var minutes = (int)Math.Floor((float)_lastRemainingTime / 60);
            var seconds = _lastRemainingTime % 60;
            _lastTimeSPan = new TimeSpan(0,0, minutes, seconds);
            
            return _lastTimeSPan;
        }
        
        private void OnDrawGizmos()
        {
            if (config == null)
                return;

            Gizmos.DrawWireSphere(transform.position, config.ExtractionRadius);
        }
    }
}