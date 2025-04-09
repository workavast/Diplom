using App.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace App.Ai.FSMs.Movement
{
    public class RandomMove : MovementState
    {
        private readonly NavMeshPath _path = new();
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
            if (NavMesh.CalculatePath(Entity.transform.position, _targetPosition, NavMesh.AllAreas, _path))
            {
                if (_path.corners.Length > 1)
                {
                    // Берем следующую точку пути
                    var nextPoint = _path.corners[1];
                
                    // Направление к следующей точке
                    var direction = (nextPoint - Entity.transform.position).normalized;

                    // Отображение пути в редакторе
                    Debug.DrawLine(Entity.transform.position, nextPoint, Color.red);

                    Entity.CalculateVelocity(direction.x, direction.z, false);
                }
            }
            
            // var direction = (_targetPosition - Entity.transform.position).normalized;
            // Entity.CalculateVelocity(direction.x, direction.z, false);
        }
    }
}