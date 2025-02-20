using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace App.PlayerInput.InputProviding
{
    public class RawInputProvider : MonoBehaviour
    {
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDirection { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public bool Fire { get; private set; }
        public bool Reload { get; private set; }
        public bool Aim { get; private set; }
        public bool Sprint { get; private set; }
        public bool Menu { get; private set; }

        public bool IsGamepad { get; private set; }

        private void LateUpdate()
        {
            Menu = false;
        }

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
        
        public void OnReload(InputAction.CallbackContext context)
        {
            Reload = context.phase switch
            {
                InputActionPhase.Started => true,
                InputActionPhase.Canceled => false,
                _ => Reload
            };
        }
        
        public void OnAim(InputAction.CallbackContext context)
        {
            Aim = context.phase switch
            {
                InputActionPhase.Started => true,
                InputActionPhase.Canceled => false,
                _ => Aim
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

        public void OnMenu(InputAction.CallbackContext context)
        {
            Menu = context.phase switch
            {
                InputActionPhase.Started => true,
                _ => Menu
            };
        }

        public bool MouseOverUI()
        {
            if (EventSystem.current == null)
                return true;

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}