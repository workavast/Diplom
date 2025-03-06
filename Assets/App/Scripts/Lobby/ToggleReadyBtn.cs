using App.Lobby.SessionData;
using App.NetworkRunning;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Lobby
{
    public class ToggleReadyBtn : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        [Inject] private readonly LobbySessionDataRepository _lobbySessionDataRepository;
        [Inject] private readonly NetworkRunnerProvider _runnerProvider;
        
        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                var runner = _runnerProvider.GetNetworkRunner();
                var sessionData = _lobbySessionDataRepository.GetData(runner.LocalPlayer);
                sessionData.ChangeReadyState(!sessionData.IsReady);
            });
        }
    }
}