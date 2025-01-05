using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Enemy
{
    public class EnemiesRepository
    {
        private readonly List<EnemyView> _playerViews = new();

        public event Action<EnemyView> OnAdd;
        public event Action<EnemyView> OnRemove;

        public void Add(EnemyView enemyView)
        {
            if (_playerViews.Contains(enemyView))
            {
                Debug.LogError($"Duplicate exception: {enemyView}");
                return;
            }
            else
            {
                _playerViews.Add(enemyView);
            }

            OnAdd?.Invoke(enemyView);
        }

        public void Remove(EnemyView enemyView)
        {
            _playerViews.Remove(enemyView);
            OnRemove?.Invoke(enemyView);
        }
    }
}