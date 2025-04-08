using App.Entities;
using UnityEngine;

namespace App.Ai.FSMs.Weapon
{
    public class Shooting : WeaponState
    {
        private readonly int _minShotsCount;
        private readonly int _maxShotsCount;
        
        private int _targetShotsCount;
        private int _initialShotsCount;
        
        public Shooting(NetEntity entity, int minShotsCount, int maxShotsCount) 
            : base(entity)
        {
            _minShotsCount = minShotsCount;
            _maxShotsCount = maxShotsCount;
        }

        protected override void OnEnterState()
        {
            _initialShotsCount = Entity.CurrentAmmo;
            _targetShotsCount = Random.Range(_minShotsCount, _maxShotsCount + 1);
        }

        protected override void OnFixedUpdate()
        {
            Entity.TryShoot();
            
            if (Entity.CurrentAmmo <= 0)
            {
                TryActivateState<Reload>();
                return;
            }
            
            if (_initialShotsCount - Entity.CurrentAmmo >= _targetShotsCount)
            {
                TryActivateState<Pause>();
                return;
            }
        }
    }
}