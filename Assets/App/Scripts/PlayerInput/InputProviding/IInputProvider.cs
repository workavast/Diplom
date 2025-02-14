using Fusion;
using UnityEngine;

namespace App.PlayerInput.InputProviding
{
    public interface IInputProvider
    {
        public Vector2 MoveDirection { get; }
        public bool Fire { get; }
        public bool Aim { get; }
        public bool Sprint { get; }
        public bool Esc { get; }
        public bool IsGamepad { get; }

        Vector2 GetLookDirection(PlayerRef playerRef);
        bool MouseOverUI();
    }
}