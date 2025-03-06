using App.Lobby.SessionData;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Lobby
{
    public class PlayersReadyView : MonoBehaviour
    {
        [SerializeField] private PlayerReadyView playerReadyViewPrefab;
        [SerializeField] private Transform holder;
        
        [Inject] private LobbySessionDataRepository _lobbySessionDataRepository;
        
        private void Awake()
        {
            _lobbySessionDataRepository.OnAdd += AddView;
        }

        private void AddView(PlayerRef playerRef, NetLobbySessionData netLobbySessionData)
        {
            var playerReadyView = Instantiate(playerReadyViewPrefab, holder);
            playerReadyView.SetSessionData(netLobbySessionData);
        }
    }
}