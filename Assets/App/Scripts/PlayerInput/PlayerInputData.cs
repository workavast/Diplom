using Fusion;
using UnityEngine;

namespace App.PlayerInput
{
    public struct PlayerInputData : INetworkInput
    {
        public float HorizontalInput;
        public float VerticalInput;
        public Vector2 LookDirection;
        public NetworkButtons Buttons;
    }
}