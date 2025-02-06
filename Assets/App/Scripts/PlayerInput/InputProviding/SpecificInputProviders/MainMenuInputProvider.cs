using Fusion;
using UnityEngine;

namespace App.PlayerInput.InputProviding
{
    public class MainMenuInputProvider : InputProviderBase
    {
        public MainMenuInputProvider(RawInputProvider rawInputProvider, MainInputProvider mainInputProvider)
            : base(rawInputProvider)
        {
            mainInputProvider.SetInputProvider(this);
        }
        
        public override Vector2 GetLookDirection(PlayerRef playerRef) 
            => default;
    }
}