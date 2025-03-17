using App.NetworkRunning;
using Avastrad.Vector2Extension;
using UnityEngine;
using Zenject;

namespace App.Entities.Player
{
    public class PlayerFollower : MonoBehaviour
    {
        [Inject] private readonly NetworkRunnerProvider _networkRunnerProvider;
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;

        private void Update()
        {
            if (_playersEntitiesRepository.TryGet(_networkRunnerProvider.GetNetworkRunner().LocalPlayer,
                    out var player))
                transform.position = player.transform.position.X0Z(transform.position.y);
        }
    }
}