using Fusion;
using UnityEngine;

namespace App.PlayerInput.InputProviding
{
    public class MainInputProvider : IInputProvider
    {
        public Vector2 MoveDirection => _inputProviderBase.MoveDirection;
        public bool Fire => _inputProviderBase.Fire;
        public bool Sprint => _inputProviderBase.Sprint;
        public bool Esc => _inputProviderBase.Esc;
        public bool IsGamepad => _inputProviderBase.IsGamepad;

        private InputProviderBase _inputProviderBase;

        public MainInputProvider(DefaultInputProvider defaultInputProvider)
            => _inputProviderBase = defaultInputProvider;

        public Vector2 GetLookDirection(PlayerRef playerRef) 
            => _inputProviderBase.GetLookDirection(playerRef);

        public void SetInputProvider(InputProviderBase inputProviderBase) 
            => _inputProviderBase = inputProviderBase;
    }
}