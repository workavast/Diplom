using UnityEngine;

namespace App.Audio.Sources
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbienceSource : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private void Awake() 
            => _audioSource = GetComponent<AudioSource>();

        private void Pause() 
            => _audioSource.Pause();

        private void Continue() 
            => _audioSource.Play();
    }
}