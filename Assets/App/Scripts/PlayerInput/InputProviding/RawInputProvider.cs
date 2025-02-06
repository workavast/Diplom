using UnityEngine;
using UnityEngine.InputSystem;

namespace App.PlayerInput.InputProviding
{
    public class RawInputProvider : MonoBehaviour
    {
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDirection { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public bool Fire { get; private set; }
        public bool Sprint { get; private set; }
        public bool Esc { get; private set; }

        public bool IsGamepad { get; private set; }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            switch (context.control.device)
            {
                case Gamepad://TODO: it doesnt detect gamepad
                    IsGamepad = true;
                    LookDirection = context.ReadValue<Vector2>();
                    MousePosition = default;
                    break;
                default:
                    IsGamepad = false;
                    LookDirection = default;
                    MousePosition = context.ReadValue<Vector2>();
                    break;
            }
        }
        
        public void OnFire(InputAction.CallbackContext context)
        {
            Fire = context.phase switch
            {
                InputActionPhase.Started => true,
                InputActionPhase.Canceled => false,
                _ => Fire
            };
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.control.device)
            {
                case Gamepad:
                    Sprint = !Sprint;
                    break;
                default:
                    Sprint = context.phase switch
                    {
                        InputActionPhase.Started => true,
                        InputActionPhase.Canceled => false,
                        _ => Sprint
                    };
                    break;
            }
        }
    }
}