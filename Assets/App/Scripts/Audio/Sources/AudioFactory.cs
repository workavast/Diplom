using System;
using System.Collections.Generic;
using Avastrad.PoolSystem;
using UnityEngine;
using Zenject;

namespace App.Audio.Sources
{
    public class AudioFactory
    {
        private readonly Transform _mainParent;
        private readonly DiContainer _diContainer;
        private readonly Dictionary<string, Transform> _parents = new();
        private readonly Dictionary<string, Pool<AudioSourceHolderPoolable>> _pools = new();
        
        public AudioFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;

            _mainParent = new GameObject { transform = { name = "Audio" } }.transform;
        }

        public AudioSourceHolderPoolable Create(AudioSourceHolderPoolable prefab, Vector3 position)
        {
            var key = prefab.gameObject.name;
            return Create(key, prefab, position);
        }
        
        public AudioSourceHolderPoolable Create(AudioSourceHolderPoolable prefab, Vector3 position, float pitch)
        {
            var key = prefab.gameObject.name;
            var audioSource = Create(key, prefab, position);
            audioSource.SetPitch(pitch);
            return audioSource;
        }
        
        public AudioSourceHolderPoolable Create(string key, AudioSourceHolderPoolable prefab, Vector3 position)
        {
            if (!_pools.ContainsKey(key))
            {
                _pools.Add(key, new Pool<AudioSourceHolderPoolable>(null));

                var parent = new GameObject { transform = { name = key , parent = _mainParent} }.transform;
                _parents.Add(key, parent);
            }
            
            Func<AudioSourceHolderPoolable> delegat = () => InstantiateEntity(prefab, key);
            _pools[key].ExtractElement(out var element, delegat);
            element.transform.position = position;

            return element;
        }

        private AudioSourceHolderPoolable InstantiateEntity(AudioSourceHolderPoolable prefab, string key)
        {
            var someAudioSource = _diContainer.InstantiatePrefab(prefab, _parents[key]).GetComponent<AudioSourceHolderPoolable>();
            return someAudioSource;
        }
    }
}