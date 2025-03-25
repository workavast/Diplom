using System;
using App.Armor;
using App.Entities;

namespace App.Damage
{
    public class DamageApplicatorFactory
    {
        private readonly bool _hasPlayersFriendlyFire;
        private readonly bool _hasEnemiesFriendlyFire;
        
        public DamageApplicatorFactory(bool hasPlayersFriendlyFire, bool hasEnemiesFriendlyFire)
        {
            _hasPlayersFriendlyFire = hasPlayersFriendlyFire;
            _hasEnemiesFriendlyFire = hasEnemiesFriendlyFire;
        }

        public IDamageApplicator CreateDamageApplicator(EntityType owner)
        {
            return owner switch
            {
                EntityType.Default => CreateEnemyDamageApplicator(),
                EntityType.Player => CreatePlayerDamageApplicator(),
                _ => throw new ArgumentOutOfRangeException(nameof(owner), owner, null)
            };
        }

        private IDamageApplicator CreatePlayerDamageApplicator() 
            => new PlayerDamageApplicator(_hasPlayersFriendlyFire);

        private IDamageApplicator CreateEnemyDamageApplicator() 
            => new EnemyDamageApplicator(_hasEnemiesFriendlyFire);
    }
}