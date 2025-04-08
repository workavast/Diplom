using App.Entities;
using UnityEngine;

namespace App.Ai.FSMs.Movement
{
    public class RandomMove : MovementState
    {
        private readonly float _minDistance;
        private readonly float _maxDistance;
        private readonly float _tolerance;

        private Vector3 _targetPosition;
        
        public RandomMove(NetEntity entity, float minDistance, float maxDistance, float tolerance) 
            : base(entity)
        {
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            _tolerance = tolerance;
        }

        protected override void OnEnterState()
        {
            var moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            var moveDistance = Random.Range(_minDistance, _maxDistance);
            _targetPosition = Entity.transform.position + moveDirection * moveDistance;
        }

        protected override void OnFixedUpdate()
        {
            if (ArrivePosition(_targetPosition, _tolerance))
            {
                TryActivateState<Stay>();
                return;
            }

            MoveToTargetPosition();
        }
        
        private void MoveToTargetPosition()
        {
            var direction = (_targetPosition - Entity.transform.position).normalized;
            Entity.CalculateVelocity(direction.x, direction.z, false);
        }
    }
}