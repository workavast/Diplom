using App.PlayerEntities;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerInput.InputProviding
{
    public class GameplayInputProvider : InputProviderBase
    {
        [Inject] private readonly PlayersRepository _playersRepository;

        public GameplayInputProvider(RawInputProvider rawInputProvider, MainInputProvider mainInputProvider)
            : base(rawInputProvider)
        {
            mainInputProvider.SetInputProvider(this);
        }
        
        public override Vector2 GetLookDirection(PlayerRef playerRef)
        {
            if (_rawInputProvider.IsGamepad)
            {
                return _rawInputProvider.LookDirection;
            }
            else
            {
                if (_playersRepository.TryGet(playerRef, out var player))
                {
                    var lookPoint = Camera.main.ScreenToWorldPoint(_rawInputProvider.MousePosition.XY0());
                    lookPoint.y = player.transform.position.y;

                    var lookDirection = (lookPoint - player.transform.position).normalized;
                    
                    return lookDirection.XZ();   
                }
            }

            return default;
        }
    }
}