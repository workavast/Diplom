using Fusion;
using UnityEngine;

namespace App.PlayerInput.InputProviding
{
    public abstract class InputProviderBase
    {
        protected readonly RawInputProvider _rawInputProvider;
        
        public Vector2 MoveDirection => _rawInputProvider.MoveDirection;
        public Vector2 LookDirection => _rawInputProvider.LookDirection;
        public Vector2 MousePosition => _rawInputProvider.MousePosition;
        public bool Fire => _rawInputProvider.Fire;
        public bool Reload => _rawInputProvider.Reload;
        public bool Aim => _rawInputProvider.Aim;
        public bool Sprint => _rawInputProvider.Sprint;
        public bool Menu => _rawInputProvider.Menu;

        public bool IsGamepad => _rawInputProvider.IsGamepad;

        protected InputProviderBase(RawInputProvider rawInputProvider)
        {
            _rawInputProvider = rawInputProvider;
        }

        public abstract Vector2 GetLookDirection(PlayerRef playerRef);

        public bool MouseOverUI() 
            => _rawInputProvider.MouseOverUI();
    }
}