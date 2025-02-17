using System;
using UnityEngine;
using UnityEngine.Audio;

namespace App.Audio
{
    public class AudioVolumeChanger
    {
        private const string MasterParam = "MasterVolume";
        private const string EffectsParam = "EffectsVolume";
        private const string MusicParam = "MusicVolume";
        
        private readonly AudioMixer _mixer;
        private readonly VolumeSettings _volumeSettings;

        public float MasterVolume => _volumeSettings.Master;
        public float EffectsVolume => _volumeSettings.EffectsVolume;
        public float MusicVolume => _volumeSettings.MusicVolume;
        
        public AudioVolumeChanger(AudioMixer mixer, VolumeSettings volumeSettings)
        {
            _mixer = mixer;
            _volumeSettings = volumeSettings;
        }

        public void StartInit()
        {         
            SetVolume(MasterParam, MasterVolume);
            SetVolume(EffectsParam, EffectsVolume);
            SetVolume(MusicParam, MusicVolume);
        }

        public float GetVolume(VolumeType volumeType)
        {
            return volumeType switch
            {
                VolumeType.Master => MasterVolume,
                VolumeType.Effects => EffectsVolume,
                VolumeType.Music => MusicVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null)
            };
        }
        
        /// <summary>
        /// Dont forgot apply changes by <see cref="Apply"/>
        /// </summary>
        public void SetVolume(VolumeType volumeType, float newVolume)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    SetMasterVolume(newVolume);
                    break;
                case VolumeType.Effects:
                    SetEffectsVolume(newVolume);
                    break;
                case VolumeType.Music:
                    SetMusicVolume(newVolume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null);
            }
        }
        
        /// <summary>
        /// Dont forgot apply changes by <see cref="Apply"/>
        /// </summary>
        public void SetMasterVolume(float newVolume)
        {
            _volumeSettings.ChangeMasterVolume(newVolume);
            SetVolume(MasterParam, MasterVolume);
        }
        
        /// <summary>
        /// Dont forgot apply changes by <see cref="Apply"/>
        /// </summary>
        public void SetEffectsVolume(float newVolume)
        {
            _volumeSettings.ChangeEffectsVolume(newVolume);
            SetVolume(EffectsParam, EffectsVolume);
        }

        /// <summary>
        /// Dont forgot apply changes by <see cref="Apply"/>
        /// </summary>
        public void SetMusicVolume(float newVolume)
        {
            _volumeSettings.ChangeMusicVolume(newVolume);
            SetVolume(MusicParam, MusicVolume);
        }

        /// <summary>
        /// used to save changes, but we dont have saves
        /// </summary>
        public void Apply() 
            => _volumeSettings.Apply();

        public void Revert()
        {
            _volumeSettings.Revert();
            
            SetVolume(MasterParam, MasterVolume);
            SetVolume(EffectsParam, EffectsVolume);
            SetVolume(MusicParam, MusicVolume);
        }

        private void SetVolume(string paramName, float newVolume)
            => _mixer.SetFloat($"{paramName}", Mathf.Lerp(-80, 0, Mathf.Sqrt(Mathf.Sqrt(newVolume))));
    }
}