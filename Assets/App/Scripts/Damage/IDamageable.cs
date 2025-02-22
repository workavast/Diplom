using App.Entities;
using Fusion;

namespace App.Damage
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, IEntity shooter); //shooter need to give him points
    }
}