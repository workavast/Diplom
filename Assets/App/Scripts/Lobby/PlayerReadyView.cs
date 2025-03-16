using App.Lobby.SessionData;
using UnityEngine;

namespace App.Lobby
{
    public class PlayerReadyView : MonoBehaviour
    {
        [SerializeField] private GameObject readyMark;
        
        private NetLobbySessionData _lobbySessionData;
        
        public void SetSessionData(NetLobbySessionData lobbySessionData)
        {
            if (_lobbySessionData != null)
            {
                _lobbySessionData.OnReadyStateChanged -= UpdateReadyStateView;
                _lobbySessionData.OnDespawned -= DestroySelf;
            }

            _lobbySessionData = lobbySessionData;
            _lobbySessionData.OnReadyStateChanged += UpdateReadyStateView;
            _lobbySessionData.OnDespawned += DestroySelf;

            UpdateReadyStateView();
        }

        private void OnDestroy()
        {
            if (_lobbySessionData != null)
            {
                _lobbySessionData.OnReadyStateChanged -= UpdateReadyStateView;
                _lobbySessionData.OnDespawned -= DestroySelf;
            }
        }

        private void DestroySelf() 
            => Destroy(gameObject);
        
        private void UpdateReadyStateView() 
            => readyMark.SetActive(_lobbySessionData.IsReady);
    }
}