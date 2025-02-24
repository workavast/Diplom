using App.Entities.Player;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.PlayerInput.InputProviding
{
    public class GameplayInputProvider : InputProviderBase
    {
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;

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
                if (_playersEntitiesRepository.TryGet(playerRef, out var player))
                {
                    var depthOffset = Camera.main.transform.position.y - player.transform.position.y;
                    var screenPoint = _rawInputProvider.MousePosition.XY0(depthOffset);
                    var lookPoint = Camera.main.ScreenToWorldPoint(screenPoint);
                    lookPoint.y = player.transform.position.y;

                    var lookDirection = (lookPoint - player.transform.position).normalized;
                    
                    return lookDirection.XZ();   
                }
            }

            return default;
        }
    }
}