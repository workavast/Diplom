using App.PlayerInput.InputProviding;
using Avastrad.UI.UiSystem;
using UnityEngine;
using Zenject;

namespace App.UI.Screens
{
    public class ByMenuButtonScreenToggle : MonoBehaviour
    {
        [SerializeField] private ScreenType screenType;

        [Inject] private ScreensController _screensController;
        [Inject] private IInputProvider _inputProvider;

        private void Update()
        {
            if (_inputProvider.Menu) 
                _screensController.ToggleScreen(screenType);
        }
    }
}