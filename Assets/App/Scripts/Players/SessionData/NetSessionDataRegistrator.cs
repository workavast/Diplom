using App.Players.Repository;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Players.SessionData
{
    public abstract class NetSessionDataRegistrator<T> : NetworkBehaviour
        where T : NetworkBehaviour
    {
        [SerializeField] private T globalSessionDataPrefab;

        [Inject] private readonly IPlayersSessionDataRepository<T> _playersSessionDataRepository;
        [Inject] private readonly IReadOnlyPlayersRepository _playersRepository;

        public override void Spawned()
        {
            if (!HasStateAuthority)
                return;

            foreach (var activePlayer in _playersRepository.Players)
            {
                if (!_playersSessionDataRepository.ContainsKey(activePlayer))
                    CreateSessionData(activePlayer);
            }

            _playersRepository.OnPlayerJoined += CreateSessionData;
            _playersRepository.OnPlayerLeft += DeleteSessionData;
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersRepository.OnPlayerJoined -= CreateSessionData;
            _playersRepository.OnPlayerLeft -= DeleteSessionData;
        }

        private void CreateSessionData(PlayerRef playerRef)
        {
            if (!HasStateAuthority)
                return;
            
            if (_playersSessionDataRepository.ContainsKey(playerRef))
            {
                Debug.LogWarning($"Session data [{GetType()}] already register for the player: {playerRef}");
                return;
            }

            Debug.Log($"Create session data [{GetType()}] for the player: {playerRef}");
            Runner.Spawn(globalSessionDataPrefab, Vector3.zero, Quaternion.identity, playerRef);
        }

        private void DeleteSessionData(PlayerRef playerRef)
        {
            if (!HasStateAuthority)
                return;

            if (_playersSessionDataRepository.ContainsKey(playerRef))
            {
                Debug.Log($"Despawn session data [{GetType()}] for the player: {playerRef}");
                Runner.Despawn(_playersSessionDataRepository.GetData(playerRef).Object);
            }
            else
                Debug.LogWarning($"Cant find session data for this player: {playerRef} | {playerRef.PlayerId}");
        }
    }
}
