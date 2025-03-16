using App.Lobby.SessionData;
using App.Players.Nicknames;
using App.Players.SessionData.Global;
using App.UI;
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
        [Inject] private GlobalSessionDataRepository _globalSessionDataRepository;
        [Inject] private NicknamesProvider _nicknamesProvider;
        
        private void Awake()
        {
            _lobbySessionDataRepository.OnAdd += AddView;
        }

        private void AddView(PlayerRef playerRef, NetLobbySessionData netLobbySessionData)
        {
            var playerReadyView = Instantiate(playerReadyViewPrefab, holder);
            playerReadyView.SetSessionData(netLobbySessionData);
            
            playerReadyView.GetComponent<NickNameView>().SetSessionData(_nicknamesProvider, _globalSessionDataRepository.GetData(playerRef));
        }
    }
}