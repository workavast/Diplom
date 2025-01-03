using System.Collections.Generic;
using UnityEngine;

namespace BlackRed.Game.Players
{
    public class PlayerSpawnPointsProvider : MonoBehaviour
    {
        [SerializeField] private float checkRadius = 1;
        [SerializeField] private LayerMask playerLayers;
        [SerializeField] private List<Transform> spawnPoints;

        public Transform GetRandomFreeSpawnPoint()
        {
            if (spawnPoints.Count == 0)
            {
                Debug.LogError("Doesnt have any spawn points");
                return transform;
            }
            
            var freePoints = new List<int>(spawnPoints.Count);
            for (var i = 0; i < spawnPoints.Count; i++) 
                freePoints.Add(i);

            while (freePoints.Count > 0)
            {
                var freePointIndex = freePoints.RandomIndex();
                var freePoint = freePoints[freePointIndex];
                var spawnPoint = spawnPoints[freePoint];

                var isHit = Physics.CheckSphere(spawnPoint.position, checkRadius, playerLayers);
                if (isHit)
                    freePoints.Remove(freePointIndex);
                else
                    return spawnPoint;
            }
            
            Debug.LogError("Doesnt have free spawn point");
            return GetRandomSpawnPoint();
        }

        public Transform GetRandomSpawnPoint()
        {
            if (spawnPoints.Count == 0)
            {
                Debug.LogError("Doesnt have any spawn points");
                return transform;
            }

            return spawnPoints.RandomValue();
        }

        private void OnDrawGizmosSelected()
        {
            if (spawnPoints == null)
                return;

            Gizmos.color = Color.red;
            foreach (var spawnPoint in spawnPoints) 
                Gizmos.DrawSphere(spawnPoint.position, checkRadius);
        }
    }
}