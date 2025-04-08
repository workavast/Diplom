using Fusion;
using UnityEngine;

namespace App.Entities.Enemy
{
    public class NetEnemiesSpawner : NetworkBehaviour
    {
        [SerializeField] private NetEnemy netEnemyPrefab;
        
        public override void Spawned()
        {
            var spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
            foreach (var spawnPoint in spawnPoints)
                Runner.Spawn(netEnemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}