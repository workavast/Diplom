using App.Entities;
using Avastrad.Vector2Extension;
using DCFApixels;
using UnityEngine;
using UnityEngine.AI;

namespace App.Ai
{
    public static class AiNetEntityExt
    {
        public static void DrawPath(this NetEntity entity, NavMeshPath path, float duration = 5)
        {
            var prevPoint = entity.transform.position;
            const float dotRadius = 0.1f;
            foreach (var corner in path.corners)
            {
                DebugX.Draw(duration, Color.red).Sphere(corner, dotRadius).Line(prevPoint, corner);
                prevPoint = corner;
            }
        }

        /// <returns> True if either a complete or partial path is found. False otherwise. </returns>
        public static bool GetPath(this NetEntity netEntity, Vector3 targetPosition, NavMeshPath path) 
            => NavMesh.CalculatePath(netEntity.transform.position, targetPosition, NavMesh.AllAreas, path);

        /// <returns> True if arrive end point </returns>
        public static bool MoveToPoint(this NetEntity netEntity, NavMeshPath path, float tolerance, ref int currentCorner)
        {
            if (currentCorner >= path.corners.Length)
                return true;

            var point = path.corners[currentCorner];
            if (netEntity.IsArrive(point, tolerance))
            {
                currentCorner++;

                if (currentCorner >= path.corners.Length)
                    return true;

                point = path.corners[currentCorner];
            }
            
            var direction = (point - netEntity.transform.position).normalized;

            DebugX.Draw(Color.red).Line(netEntity.transform.position, point);

            netEntity.CalculateVelocity(direction.x, direction.z, true);
            
            return false;
        }

        public static void LookAtPoint(this NetEntity netEntity, Vector3 targetPosition)
        {
            var direction = (targetPosition - netEntity.transform.position).normalized;
            netEntity.RotateByLookDirection(direction.XZ());
        }

        /// <summary> Check arriving by XZ coordinates </summary>
        public static bool IsArrive(this NetEntity netEntity, Vector3 targetPosition, float tolerance) 
            => netEntity.IsArrive(targetPosition.XZ(), tolerance);
        
        /// <summary> Check arriving by XZ coordinates </summary>
        public static bool IsArrive(this NetEntity netEntity, Vector2 targetPosition, float tolerance) 
            => Vector2.Distance(netEntity.transform.position.XZ(), targetPosition) <= tolerance;
    }
}