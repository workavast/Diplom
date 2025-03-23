using Avastrad.UI.UiSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI.Screens
{
    [RequireComponent(typeof(Button))]
    public class SetScreenBtn : MonoBehaviour
    {
        [SerializeField] private ScreenType screenType;

        [Inject] private readonly ScreensController _screensController;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SetScreen);
        }

        private void SetScreen() 
            => _screensController.SetScreen(screenType);
    }
}