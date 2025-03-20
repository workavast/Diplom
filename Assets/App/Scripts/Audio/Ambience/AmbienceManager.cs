using System.Collections;
using UnityEngine;

namespace App.Audio.Ambience
{
    public class AmbienceManager : MonoBehaviour
    {
        private AmbienceSource _currentAmbience;
        private Coroutine _transitionCoroutine;
    
        private void Awake() 
            => DontDestroyOnLoad(gameObject);

        public void Activate(AmbienceSource newAmbiencePrefab, float transitionTime = 1f)
        {
            if (newAmbiencePrefab == null)
            {
                Debug.LogError("You try activate null ambience");
                return;
            }

            if (_currentAmbience != null && _currentAmbience.name == newAmbiencePrefab.name)
                return;

            if (_transitionCoroutine != null) 
                StopCoroutine(_transitionCoroutine);
            
            var newAmbience = Instantiate(newAmbiencePrefab, transform);
            newAmbience.name = newAmbiencePrefab.name;
            _transitionCoroutine = StartCoroutine(TransitionAmbience(newAmbience, transitionTime));
        }
    
        private IEnumerator TransitionAmbience(AmbienceSource newAmbience, float duration)
        {
            var time = 0f;
        
            var newSource = newAmbience.AudioSource;
            newSource.volume = 0f;
            newSource.Play();

            AudioSource prevSource = null;
            if (_currentAmbience != null) 
                prevSource = _currentAmbience.AudioSource;
            
            var prevSourceVolume = prevSource != null ? prevSource.volume : 0f;
            var newSourceDefaultVolume = newAmbience.DefaultVolume;
        
            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration;
            
                if (prevSource != null)
                    prevSource.volume = Mathf.Lerp(prevSourceVolume, 0f, t);
            
                newSource.volume = Mathf.Lerp(0f, newSourceDefaultVolume, t);
            
                yield return null;
            }
        
            if (_currentAmbience != null) 
                Destroy(_currentAmbience.gameObject);

            _currentAmbience = newAmbience;
        }
    }
}