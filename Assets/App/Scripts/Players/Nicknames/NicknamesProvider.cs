using App.Players.SessionData;
using App.Players.SessionData.Global;
using Fusion;

namespace App.Players.Nicknames
{
    public class NicknamesProvider
    {
        private readonly IPlayersSessionDataRepository<NetGlobalSessionData> _playerSessionDatasRepository;

        public NicknamesProvider(IPlayersSessionDataRepository<NetGlobalSessionData> playerSessionDatasRepository)
        {
            _playerSessionDatasRepository = playerSessionDatasRepository;
        }
        
        public string GetNickName(PlayerRef playerRef) 
            => _playerSessionDatasRepository.GetData(playerRef).NickName.Value;
    }
}