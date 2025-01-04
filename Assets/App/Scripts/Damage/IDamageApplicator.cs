using App.PlayerEntities;
using Fusion;

namespace App.Damage
{
    public interface IDamageApplicator
    {
        public bool FriendlyFire { get; }

        public void TryApplyDamage(int damage, NetPlayerController receiver, PlayerRef shooter);

        public void ApplyDamage(int damage, IDamageable damageable, PlayerRef shooter);
    }
}