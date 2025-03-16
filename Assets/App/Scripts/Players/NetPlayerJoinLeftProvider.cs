using App.Players.Repository;
using Fusion;
using Zenject;

namespace App.Players
{
    public class NetPlayersJoinLeftProvider : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [Inject] private readonly PlayersRepository _playersRepository;

        public override void Spawned()
        {
            foreach (var activePlayer in Runner.ActivePlayers) 
                PlayerJoined(activePlayer);
        }

        public void PlayerJoined(PlayerRef playerRef) 
            => _playersRepository.PlayerJoined(playerRef);

        public void PlayerLeft(PlayerRef playerRef) 
            => _playersRepository.PlayerLeft(playerRef);
    }
}