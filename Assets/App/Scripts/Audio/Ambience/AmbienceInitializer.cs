using UnityEngine;
using Zenject;

namespace App.Audio.Ambience
{
    public class AmbienceInitializer : MonoBehaviour
    {
        [SerializeField] private AmbienceSource ambienceSourcePrefab;
        
        [Inject] private readonly AmbienceManager _ambienceManager;

        private void Start() 
            => _ambienceManager.Activate(ambienceSourcePrefab);
    }
}