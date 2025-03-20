using UnityEngine;

namespace App.Audio.Ambience
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbienceSource : MonoBehaviour
    {
        public AudioSource AudioSource { get; private set; }
        public float DefaultVolume { get; private set; }
    
        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            DefaultVolume = AudioSource.volume;
        }
    }
}