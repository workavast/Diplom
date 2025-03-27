using System.Collections;
using App.Weapons;
using Fusion;
using UnityEngine;

namespace App.Entities.Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private NetEnemy netEnemyPrefab;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(2);

            if (!HasStateAuthority)
                yield break;
            
            Spawn(transform.position, transform.rotation);
        }
        
        public NetEnemy Spawn(Vector3 position, Quaternion rotation)
        {
            var netEnemy = Runner.Spawn(netEnemyPrefab, position, rotation);
            netEnemy.SetWeapon(WeaponId.Pistol);
            netEnemy.SetArmor(0);

            netEnemy.OnDeath += () => StartCoroutine(Spawn());
            
            return netEnemy;
        }
    }
}