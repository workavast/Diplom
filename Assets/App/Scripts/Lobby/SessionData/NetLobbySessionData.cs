using System;
using Fusion;
using Zenject;

namespace App.Lobby.SessionData
{
    public class NetLobbySessionData : NetworkBehaviour
    {
        [Networked] [field: ReadOnly] [OnChangedRender(nameof(ReadyStateChanged))] public bool IsReady { get; private set; }

        [Inject] private LobbySessionDataRepository _playersSessionDataRepository;
        
        public PlayerRef PlayerRef => Object.InputAuthority;
        
        public event Action OnDespawned;
        public event Action OnReadyStateChanged;

        public override void Spawned()
        {
            if (!_playersSessionDataRepository.TryRegister(PlayerRef, this))
                Runner.Despawn(GetComponent<NetworkObject>());
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playersSessionDataRepository.TryRemove(PlayerRef);
            OnDespawned?.Invoke();
        }
        
        private void ReadyStateChanged() 
            => OnReadyStateChanged?.Invoke();

        public void ChangeReadyState(bool isReady)
        {
            IsReady = isReady;
        }
    }
}