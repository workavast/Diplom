using App.Armor;
using Avastrad.EventBusFramework;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Entities.Enemy
{
    [RequireComponent(typeof(EnemyView))]
    public class NetEnemy : NetEntity
    {
        public override EntityType EntityType => EntityType.Default;
        
        protected override IEventBus EventBus { get; set; }
        protected override ArmorsConfig ArmorsConfig { get; set; }

        private EnemiesRepository _enemiesRepository;

        [Inject]
        public void Construct(EnemiesRepository enemiesRepository, IEventBus eventBus, ArmorsConfig armorsConfig)
        {
            _enemiesRepository = enemiesRepository;
            EventBus = eventBus;
            ArmorsConfig = armorsConfig;
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
