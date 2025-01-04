using Fusion;

namespace App.Damage
{
    public interface IDamageable
    {
        public void TakeDamage(int damage, PlayerRef shooter); //shooter need to give him points
    }
}