using System;
using TMPro;
using UnityEngine;

namespace App.Settings
{
    public class ResolutionSettingsController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private InspectorResolution[] resolutions;

        private Resolution _defaultResolution;

        private void Awake()
        {
            _defaultResolution = Screen.currentResolution;
        }

        private void Start()
        {
            resolutionDropdown.ClearOptions();

            var currentResolutionIndex = 0;
            for (var i = 0; i < resolutions.Length; i++)
            {
                var resolution = resolutions[i];
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.Width + " x " + resolution.Height));

                if (resolution.Width == Screen.currentResolution.width && resolution.Height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void Apply()
        {
            var selectedResolution = resolutions[resolutionDropdown.value];
            Screen.SetResolution(selectedResolution.Width, selectedResolution.Height, Screen.fullScreen);
        }

        public void ResetToDefault()
        {
            Screen.SetResolution(_defaultResolution.width, _defaultResolution.height, Screen.fullScreen);
            var resolutionIndex = 0;
            for (var i = 0; i < resolutions.Length; i++)
            {
                var resolution = resolutions[i];
                if (resolution.Width == _defaultResolution.width && resolution.Height == _defaultResolution.height)
                {
                    resolutionIndex = i;
                    break;
                }
            }
            resolutionDropdown.value = resolutionIndex;
        }
        
        [Serializable]
        private struct InspectorResolution
        {
            [field: SerializeField, Min(0)] public int Width { get; private set; }
            [field: SerializeField, Min(0)] public int Height { get; private set; }
        }
    }
}