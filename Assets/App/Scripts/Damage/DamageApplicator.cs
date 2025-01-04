using App.PlayerEntities;
using Fusion;

namespace App.Damage
{
    public class DamageApplicator : IDamageApplicator
    {
        public bool FriendlyFire { get; }

        public DamageApplicator(bool hasFriendlyFire)
        {
            FriendlyFire = hasFriendlyFire;
        }
        
        public void TryApplyDamage(int damage, NetPlayerController receiver, PlayerRef shooter)
        {
            if (FriendlyFire) 
                receiver.TakeDamage(damage, shooter);
        }
        
        public void ApplyDamage(int damage, IDamageable damageable, PlayerRef shooter)
        {
            damageable.TakeDamage(damage, shooter);
        }
    }
}