using UnityEngine;

namespace App.Audio.Sources
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceHolder : MonoBehaviour
    {
        protected AudioSource _audioSource;
        
        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Play()
        {
            _audioSource.Stop();
            _audioSource.Play();
        }

        public void SetPitch(float newPitch) 
            => _audioSource.pitch = newPitch;

        public virtual void SetClip(AudioClip audioClip, bool play = true)
        {
            _audioSource.clip = audioClip;

            if (play)
                _audioSource.Play();
        }
        
        private void Pause() 
            => _audioSource.Pause();

        private void Continue() 
            => _audioSource.Play();
    }
}