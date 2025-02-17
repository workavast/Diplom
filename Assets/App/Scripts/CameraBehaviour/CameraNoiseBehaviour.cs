using App.Players;
using Avastrad.CustomTimer;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace App.CameraBehaviour
{
    public class CameraNoiseBehaviour : MonoBehaviour
    {
        [SerializeField] private CinemachineBasicMultiChannelPerlin perlin;
        [SerializeField] private NoiseConfig shotConfig;
        
        [Inject] private readonly LocalPlayerProvider _localPlayerProvider;

        private readonly Timer _noiseTimer = new(1, 1);

        private void Awake()
        {
            _localPlayerProvider.OnWeaponShot += ShotShake;

            perlin.NoiseProfile = shotConfig.NoiseSettings;
            perlin.AmplitudeGain = 0;
        }

        private void Update()
        {
            if (_noiseTimer.TimerIsEnd)
                return;
            
            _noiseTimer.Tick(Time.deltaTime);
            perlin.AmplitudeGain = 1 - _noiseTimer.CurrentTime / _noiseTimer.MaxTime;
        }

        private void ShotShake()
        {
            perlin.NoiseProfile = shotConfig.NoiseSettings;
            _noiseTimer.SetMaxTime(shotConfig.TimeLenght);
        }
    }
}