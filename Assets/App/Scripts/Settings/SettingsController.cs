using App.Audio;
using UnityEngine;
using Zenject;

namespace App.Settings
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private ResolutionSettingsController resolutionSettingsController;
        [SerializeField] private FPSSettingsController fpsSettingsController;
        [SerializeField] private ScreenModeSettingsController screenModeSettingsController;

        [Inject] private readonly AudioVolumeChanger _audioVolumeChanger;
        
        public void ApplySettings()
        {
            _audioVolumeChanger.Apply();
            resolutionSettingsController.Apply();
            fpsSettingsController.Apply();
            screenModeSettingsController.Apply();
        }

        public void ResetSettings()
        {
            _audioVolumeChanger.ResetToDefault();
            resolutionSettingsController.ResetToDefault();
            fpsSettingsController.ResetToDefault();
            screenModeSettingsController.ResetToDefault();
        }
    }
}
