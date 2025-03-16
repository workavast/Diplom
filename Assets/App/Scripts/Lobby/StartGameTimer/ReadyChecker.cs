using System;
using App.Lobby.SessionData;
using Avastrad.Storages;
using Fusion;

namespace App.Lobby.StartGameTimer
{
    public class ReadyChecker : IDisposable
    {
        public IReadOnlyStorage<int> ReadyCounter => _readyCounter; 

        private readonly LobbySessionDataRepository _lobbySessionDataRepository;
        private readonly IntStorage _readyCounter = new();
        
        public event Action OnDataChanged;
        
        public ReadyChecker(LobbySessionDataRepository lobbySessionDataRepository)
        {
            _lobbySessionDataRepository = lobbySessionDataRepository;
            
            _lobbySessionDataRepository.OnAdd += OnAddPlayer;
            _lobbySessionDataRepository.OnRemove += OnRemovePlayer;
            
            UpdateData();
        }

        public bool PlayerIsReady(PlayerRef playerRef)
        {
            if (!_lobbySessionDataRepository.ContainsKey(playerRef))
                return false;
            
            return _lobbySessionDataRepository.GetData(playerRef).IsReady;
        }

        private void OnAddPlayer(PlayerRef _, NetLobbySessionData netLobbySessionData)
        {
            netLobbySessionData.OnReadyStateChanged += UpdateData;
            _readyCounter.SetMaxValue(_readyCounter.MaxValue + 1);
            
            UpdateData();
        }
        
        private void OnRemovePlayer(PlayerRef _, NetLobbySessionData netLobbySessionData)
        {
            netLobbySessionData.OnReadyStateChanged -= UpdateData;
            _readyCounter.SetMaxValue(_readyCounter.MaxValue - 1);
            
            UpdateData();
        }
        
        private void UpdateData()
        {
            _readyCounter.SetCurrentValue(0);
            foreach (var lobbySessionData in _lobbySessionDataRepository.PlayersSessionData.Values)
                if (lobbySessionData.IsReady)
                    _readyCounter.ChangeCurrentValue(1);

            OnDataChanged?.Invoke();
        }

        public void Dispose()
        {
            if (_lobbySessionDataRepository == null)
                return;

            _lobbySessionDataRepository.OnAdd -= OnAddPlayer;
            _lobbySessionDataRepository.OnRemove -= OnRemovePlayer;

            foreach (var sessionData in _lobbySessionDataRepository.PlayersSessionData.Values)
                sessionData.OnReadyStateChanged -= UpdateData;
        }
    }
}