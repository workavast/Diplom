using App.Players.SessionDatas;
using Fusion;

namespace App.Players.Nicknames
{
    public class NicknamesProvider
    {
        private readonly IPlayerSessionDatasRepository _playerSessionDatasRepository;

        public NicknamesProvider(IPlayerSessionDatasRepository playerSessionDatasRepository)
        {
            _playerSessionDatasRepository = playerSessionDatasRepository;
        }

        public string GetNickName(PlayerRef playerRef) 
            => _playerSessionDatasRepository.Datas[playerRef].NickName.Value;
    }
}