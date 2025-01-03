using Fusion;
using UnityEngine;

namespace BlackRed.Game.PlayerInput
{
    public struct PlayerInputData : INetworkInput
    {
        public float HorizontalInput;
        public float VerticalInput;
        public Vector2 LookPoint;
        public NetworkButtons Buttons;
    }
}