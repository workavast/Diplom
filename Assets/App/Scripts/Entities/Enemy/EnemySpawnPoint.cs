using UnityEngine;

namespace App.Entities.Enemy
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public float arrowLength = 1.0f;
        public Color arrowColor = Color.green;
        public float arrowHeadAngle = 20.0f;
        public float arrowHeadLength = 0.3f;

        private void OnDrawGizmos()
        {
            Gizmos.color = arrowColor;

            var start = transform.position;
            var direction = transform.forward * arrowLength;
            var end = start + direction;

            Gizmos.DrawLine(start, end);
            DrawArrowHead(end, direction.normalized);
        }

        private void DrawArrowHead(Vector3 position, Vector3 direction)
        {
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;

            Gizmos.DrawLine(position, position + right * arrowHeadLength);
            Gizmos.DrawLine(position, position + left * arrowHeadLength);
        }
    }
}