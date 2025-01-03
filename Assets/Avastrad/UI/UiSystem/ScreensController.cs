using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    public class ScreensController : MonoBehaviour
    {
        private ScreensRepository _screenRepository;
        
        private void Awake()
        {
            _screenRepository = GetComponentInChildren<ScreensRepository>();
        }

        private void Start()
        {
            foreach (var screen in _screenRepository.Screens) 
                screen.Initialize();
        }

        public void SetScreens(IEnumerable<ScreenType> screenTypes)
        {
            foreach (var screen in _screenRepository.Screens)
                TryToggleScreen(screen, false);

            foreach (var screenType in screenTypes) 
                ToggleScreen(screenType, true);
        }
        
        public void SetScreen(ScreenType screenType)
        {
            foreach (var screen in _screenRepository.Screens) 
                screen.Hide();

            ToggleScreen(screenType, true);
        }
        
        public void ToggleScreen(ScreenType screenType, bool show)
        {
            var screen = _screenRepository.GetScreen(screenType);
            TryToggleScreen(screen, show);
        }

        private static void TryToggleScreen(ScreenBase screen, bool show)
        {
            if (screen.isActiveAndEnabled == show) 
                return;
            
            if (show)
                screen.Show();
            else
                screen.Hide();
        }
    }
}