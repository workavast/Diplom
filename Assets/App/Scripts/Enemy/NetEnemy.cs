using App.Entities;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Enemy
{
    [RequireComponent(typeof(EnemyView))]
    public class NetEnemy : NetEntityBase
    {
        private EnemiesRepository _enemiesRepository;

        public override EntityType EntityType => EntityType.Default;

        [Inject]
        public void Construct(EnemiesRepository enemiesRepository, IEventBus eventBus)
        {
            _enemiesRepository = enemiesRepository;
            base.Construct(eventBus);
        }

        public override void Spawned()
        {
            base.Spawned();
            _enemiesRepository.Add(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            _enemiesRepository.Remove(this);
        }

        public override string GetName()
            => nameof(NetEnemy);
    }
}
