using App.Audio;
using Avastrad.UI.Elements;
using UnityEngine;
using Zenject;

namespace App.UI
{
    [RequireComponent(typeof(ExtendedSlider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private bool autoApply;
        [SerializeField] private VolumeType volumeType;
        
        [Inject] private readonly AudioVolumeChanger _audioVolumeChanger;
        
        private ExtendedSlider _slider;
        
        private void Awake()
        {
            _slider = GetComponent<ExtendedSlider>();

            _audioVolumeChanger.OnResetToDefault += UpdateView;
            if (autoApply) 
                _slider.OnPointerUpEvent += _audioVolumeChanger.Apply;
        }

        private void Start()
        {
            _slider.value = _audioVolumeChanger.GetVolume(volumeType);
            _slider.onValueChanged.AddListener(SetVolume);
        }

        private void UpdateView() 
            => _slider.SetValueWithoutNotify(_audioVolumeChanger.GetVolume(volumeType));

        private void SetVolume(float newVolume) 
            => _audioVolumeChanger.SetVolume(volumeType, newVolume);
        
        private void OnDestroy()
        {
            _slider.OnPointerUpEvent -= _audioVolumeChanger.Apply;
            _slider.onValueChanged.RemoveListener(SetVolume);
        }
    }
}