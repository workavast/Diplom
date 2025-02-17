using System;
using Avastrad.CustomTimer;
using Avastrad.PoolSystem;
using UnityEngine;

namespace App.Audio.Sources
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceHolderPoolable : AudioSourceHolder, IPoolable<AudioSourceHolderPoolable>
    {
        private AudioSource _audioSource;
        private Timer _existTimer;
        
        public event Action<AudioSourceHolderPoolable> ReturnElementEvent;
        public event Action<AudioSourceHolderPoolable> DestroyElementEvent;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            var length = 1f;
            if (_audioSource.clip != null )
                length = _audioSource.clip.length;
            
            _existTimer = new Timer(length);
            _existTimer.OnTimerEnd += () => ReturnElementEvent?.Invoke(this);
        }

        private void Update() 
            => _existTimer.Tick(Time.deltaTime);

        private void OnDestroy() 
            => DestroyElementEvent?.Invoke(this);
        
        public override void SetClip(AudioClip audioClip, bool play = true)
        {
            base.SetClip(audioClip, play);
            
            _existTimer.SetMaxTime(_audioSource.clip.length);
        }

        public void OnElementExtractFromPool()
        {
#if UNITY_EDITOR
            gameObject.SetActive(true);
#endif

            _existTimer.Reset();
            _audioSource.Play();
        }

        public void OnElementReturnInPool()
        {
            _existTimer.SetPause();
            _audioSource.Stop();
            
#if UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }
    }
}