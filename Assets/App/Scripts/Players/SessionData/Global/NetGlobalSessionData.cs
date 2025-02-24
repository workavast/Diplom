using System;
using Fusion;
using Zenject;

namespace App.Players.SessionData.Global
{
    public class NetGlobalSessionData : NetworkBehaviour
    {
        [Networked] [field: ReadOnly] 
        [OnChangedRender(nameof(NickNameChanged))] 
        public NetworkString<_16> NickName { get; private set; }

        [Inject] private readonly GlobalSessionDataRepository _globalSessionDataRepository;

        public PlayerRef PlayerRef => Object.InputAuthority;
        
        public event Action OnDespawned;
        public event Action OnNickNameChanged;

        public override void Spawned()
        {
            if (HasInputAuthority) 
                Rpc_SetNickName(PlayerData.NickName);

            if (!_globalSessionDataRepository.TryRegister(PlayerRef, this))
                Runner.Despawn(GetComponent<NetworkObject>());
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            
            _globalSessionDataRepository.TryRemove(PlayerRef);
            OnDespawned?.Invoke();
        }
        
        private void NickNameChanged() 
            => OnNickNameChanged?.Invoke();

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void Rpc_SetNickName(string nickName)
        {
            NickName = nickName;
        }
    }
}