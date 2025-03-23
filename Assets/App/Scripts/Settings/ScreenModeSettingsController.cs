using TMPro;
using UnityEngine;

namespace App.Settings
{
    public class ScreenModeSettingsController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown screenModeDropdown;

        private void Start()
        {
            screenModeDropdown.value = Screen.fullScreen ? 1 : 0;
        }

        public void Apply()
        {
            var isFullScreen = screenModeDropdown.value == 1;
            Screen.fullScreen = isFullScreen;
        }

        public void ResetToDefault()
        {
            Screen.fullScreen = true;
            screenModeDropdown.value = 1;
        }
    }
}