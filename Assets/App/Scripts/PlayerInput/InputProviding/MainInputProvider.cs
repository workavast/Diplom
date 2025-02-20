using Fusion;
using UnityEngine;

namespace App.PlayerInput.InputProviding
{
    public class MainInputProvider : IInputProvider
    {
        public Vector2 MoveDirection => _inputProviderBase.MoveDirection;
        public bool Fire => _inputProviderBase.Fire;
        public bool Reload => _inputProviderBase.Reload;
        public bool Aim => _inputProviderBase.Aim;
        public bool Sprint => _inputProviderBase.Sprint;
        public bool Menu => _inputProviderBase.Menu;
        public bool IsGamepad => _inputProviderBase.IsGamepad;

        private InputProviderBase _inputProviderBase;

        public MainInputProvider(DefaultInputProvider defaultInputProvider)
            => _inputProviderBase = defaultInputProvider;

        public Vector2 GetLookDirection(PlayerRef playerRef) 
            => _inputProviderBase.GetLookDirection(playerRef);

        public bool MouseOverUI() 
            => _inputProviderBase.MouseOverUI();

        public void SetInputProvider(InputProviderBase inputProviderBase) 
            => _inputProviderBase = inputProviderBase;
    }
}