using System;
using App.Lobby.SessionData;
using App.Lobby.StartGameTimer.FSM;
using App.Lobby.StartGameTimer.FSM.SpecificStates;
using UnityEngine;
using Zenject;

namespace App.Lobby.StartGameTimer
{
    public class NetStartGameTimer : MonoBehaviour
    {
        [SerializeField, Min(0)] private float nonFullTeamStartTime = 90;
        [SerializeField, Min(0)] private float fullTeamStartTime = 10;
        [SerializeField] private NetStartGameTimerModel netStartGameTimerModel;

        [Inject] private readonly LobbySessionDataRepository _lobbySessionDataRepository;

        private ReadyChecker _readyChecker;
        private TimerStateMachine _stateMachine;
        
        private void Awake()
        {
            _readyChecker = new ReadyChecker(_lobbySessionDataRepository);
            _stateMachine = new TimerStateMachine(
                new TimerState[]
                {
                    new Idle(netStartGameTimerModel, _readyChecker, SetState), 
                    new IsOver(netStartGameTimerModel, _readyChecker, SetState), 
                    new LongTimer(netStartGameTimerModel, _readyChecker, SetState, nonFullTeamStartTime), 
                    new ShortTimer(netStartGameTimerModel, _readyChecker, SetState, fullTeamStartTime)
                },
                typeof(Idle)
            );
        }

        private void OnDestroy()
        {
            _stateMachine?.Dispose();
            _readyChecker?.Dispose();
        }

        private void Update() 
            => _stateMachine.OnUpdate();

        private void SetState(Type state) 
            => _stateMachine.SetState(state);
    }
}