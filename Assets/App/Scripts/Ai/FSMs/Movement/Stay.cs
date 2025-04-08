using App.Entities;
using UnityEngine;

namespace App.Ai.FSMs.Movement
{
    public class Stay : MovementState
    {
        private readonly float _minDuration;
        private readonly float _maxDuration;
        private float _targetDuration;
        
        public Stay(NetEntity entity, float minDuration, float maxDuration) 
            : base(entity)
        {
            _minDuration = minDuration;
            _maxDuration = maxDuration;
        }

        protected override void OnEnterState()
        {
            _targetDuration = Random.Range(_minDuration, _maxDuration);
        }

        protected override void OnFixedUpdate()
        {
            if (StateTime >= _targetDuration)
            {
                TryActivateState<RandomMove>();
                return;
            }
            
            Entity.CalculateVelocity(0, 0, false);
        }
    }
}