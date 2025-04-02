using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Audio.Ambience
{
    public class AmbienceManager : MonoBehaviour
    {
        private readonly List<AmbienceSource> _activeAmbiences = new(2);
        private Coroutine _transitionCoroutine;

        private void Awake()
            => DontDestroyOnLoad(gameObject);

        public void Activate(AmbienceSource ambiencePrefab, float transitionTime = 1f)
        {
            if (ambiencePrefab == null)
            {
                Debug.LogError("You tried to activate a null ambience");
                return;
            }

            if (_activeAmbiences.Any(a => a.name == ambiencePrefab.name))
                return;

            if (_transitionCoroutine != null)
                StopCoroutine(_transitionCoroutine);

            var newAmbience = Instantiate(ambiencePrefab, transform);
            newAmbience.name = ambiencePrefab.name;
            _activeAmbiences.Add(newAmbience);

            _transitionCoroutine = StartCoroutine(TransitionAmbience(newAmbience, transitionTime));
        }

        private IEnumerator TransitionAmbience(AmbienceSource newAmbience, float duration)
        {
            var time = 0f;

            var newSource = newAmbience.AudioSource;
            newSource.volume = 0f;
            newSource.Play();

            var newSourceDefaultVolume = newAmbience.DefaultVolume;

            var fadingSources = new List<AudioSource>(_activeAmbiences.Count > 1 ? _activeAmbiences.Count - 1 : 0);
            for (var i = 0; i < fadingSources.Capacity; i++) 
                fadingSources.Add(_activeAmbiences[i].AudioSource);

            // var fadingSources = _activeAmbiences
            //     .Where(a => a != newAmbience)
            //     .Select(a => a.AudioSource)
            //     .ToList();

            var initialVolumes = fadingSources.ToDictionary(s => s, s => s.volume);

            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration;

                foreach (var source in fadingSources)
                    source.volume = Mathf.Lerp(initialVolumes[source], 0f, t);

                newSource.volume = Mathf.Lerp(0f, newSourceDefaultVolume, t);

                yield return null;
            }

            foreach (var ambience in _activeAmbiences.Where(a => a != newAmbience).ToList())
            {
                Destroy(ambience.gameObject);
                _activeAmbiences.Remove(ambience);
            }
        }
    }
}