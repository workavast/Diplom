using App.CursorBehaviour;
using Avastrad.UI.UiSystem;
using Zenject;

namespace App.UI.Screens
{
    public class MainMenu : ScreenBase
    {
        [Inject] private readonly CursorVisibilityBehaviour _cursorVisibilityBehaviour;
        
        private void OnEnable()
        {
            _cursorVisibilityBehaviour.Show();
        }

        private void OnDisable()
        {
            _cursorVisibilityBehaviour.Hide();
        }   
    }
}