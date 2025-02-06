using Fusion;
using UnityEngine;

namespace App.PlayerInput.InputProviding
{
    public class DefaultInputProvider : InputProviderBase
    {
        public DefaultInputProvider(RawInputProvider rawInputProvider)
            : base(rawInputProvider) { }
        
        public override Vector2 GetLookDirection(PlayerRef playerRef) 
            => default;
    }
}