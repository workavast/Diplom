using App.NetworkRunning;
using App.PlayerEntities;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace App.CameraControlling
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineVirtualCamera;
        
        [Inject] private NetworkRunnerProvider _networkRunnerProvider;
        [Inject] private PlayersRepository _playersRepository;

        private PlayerView _playerView;
        
        private void Awake()
        {
            _playersRepository.OnPlayerAdd += OnPlayerViewAdded;
        }

        private void OnPlayerViewAdded(PlayerRef playerRef, NetPlayerController netPlayer)
        {
            if (playerRef != _networkRunnerProvider.GetNetworkRunner().LocalPlayer)
                return;
            
            _playerView = netPlayer.PlayerView;
            cinemachineVirtualCamera.LookAt = _playerView.transform;
            cinemachineVirtualCamera.Follow = _playerView.transform;
        }
    }
}