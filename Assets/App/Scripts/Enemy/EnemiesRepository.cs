using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Enemy
{
    public class EnemiesRepository
    {
        private readonly Dictionary<int, NetEnemy> _enemiesById = new();
        private readonly List<NetEnemy> _enemies = new();

        public event Action<NetEnemy> OnAdd;
        public event Action<NetEnemy> OnRemove;

        public void Add(NetEnemy enemy)
        {
            if (_enemies.Contains(enemy))
            {
                Debug.LogError($"Duplicate exception: {enemy}");
                return;
            }
            else
            {
                _enemiesById.Add(enemy.Identifier.Id, enemy);
                _enemies.Add(enemy);
            }

            OnAdd?.Invoke(enemy);
        }

        public void Remove(NetEnemy enemy)
        {
            _enemiesById.Remove(enemy.Identifier.Id);
            _enemies.Remove(enemy);
            OnRemove?.Invoke(enemy);
        }
        
        public bool TryGet(int identifier, out NetEnemy enemy)
        {
            if (_enemiesById.TryGetValue(identifier, out var value))
            {
                enemy = value;
                return true;
            }

            enemy = null;
            return false;
        }
    }
}