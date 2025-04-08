using System;
using App.Lobby.SessionData;
using App.Lobby.StartGameTimer.FSM;
using App.Lobby.StartGameTimer.FSM.SpecificStates;
using UnityEngine;
using Zenject;

namespace App.Lobby.StartGameTimer
{
    public class StartGameTimer : MonoBehaviour
    {
        [SerializeField] private NetStartGameTimerModel netModel;

        [Inject] private readonly LobbySessionDataRepository _lobbySessionDataRepository;

        private ReadyChecker _readyChecker;
        private TimerStateMachine _stateMachine;
        
        private void Awake()
        {
            _readyChecker = new ReadyChecker(_lobbySessionDataRepository);
            _stateMachine = new TimerStateMachine(
                new TimerState[]
                {
                    new Idle(netModel, _readyChecker, SetState), 
                    new IsOver(netModel, _readyChecker, SetState), 
                    new LongTimer(netModel, _readyChecker, SetState, netModel.NonFullTeamStartTime), 
                    new ShortTimer(netModel, _readyChecker, SetState, netModel.FullTeamStartTime)
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