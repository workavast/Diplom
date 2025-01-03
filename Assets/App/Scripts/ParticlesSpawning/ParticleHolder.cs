using System;
using Avastrad.CustomTimer;
using Avastrad.PoolSystem;
using UnityEngine;

namespace App.ParticlesSpawning
{
    public class ParticleHolder : MonoBehaviour, IPoolable<ParticleHolder, ParticleType>
    {
        [field: SerializeField] public ParticleType PoolId { get; private set; }
        [SerializeField] private ParticleSystem particle;
        
        public event Action<ParticleHolder> ReturnElementEvent;
        public event Action<ParticleHolder> DestroyElementEvent;

        private readonly Timer _existTimer = new(0);
        
        private void Awake()
        {
            _existTimer.SetMaxTime(particle.main.duration);
            _existTimer.OnTimerEnd += () => ReturnElementEvent?.Invoke(this);
        }

        private void Update() 
            => _existTimer.Tick(Time.deltaTime);

        public void OnElementExtractFromPool()
        {
            _existTimer.Reset();
            particle.Play();
        }

        public void OnElementReturnInPool()
        {
            // throw new NotImplementedException();
        }

        private void OnDestroy() 
            => DestroyElementEvent?.Invoke(this);
    }
}