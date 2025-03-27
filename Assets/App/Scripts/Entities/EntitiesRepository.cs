using App.Entities.Enemy;
using App.Entities.Player;
using Zenject;

namespace App.Entities
{
    public class EntitiesRepository
    {
        [Inject] private PlayersEntitiesRepository _playersEntitiesRepository;
        [Inject] private EnemiesRepository _enemiesRepository;

        public bool TryGetPlayer(EntityIdentifier identifier, out NetPlayerEntity player) 
            => _playersEntitiesRepository.TryGet(identifier.Id, out player);
        
        public bool TryGetPlayer(int identifier, out NetPlayerEntity player) 
            => _playersEntitiesRepository.TryGet(identifier, out player);
        
        public bool TryGetEnemy(EntityIdentifier identifier, out NetEnemy enemy) 
            => _enemiesRepository.TryGet(identifier.Id, out enemy);
        
        public bool TryGetEnemy(int identifier, out NetEnemy enemy) 
            => _enemiesRepository.TryGet(identifier, out enemy);

        public bool TryGet(EntityIdentifier identifier, out IEntity entity) 
            => TryGet(identifier.Id, out entity);
        
        public bool TryGet(int identifier, out IEntity entity)
        {
            if (TryGetPlayer(identifier, out var player))
            {
                entity = player;
                return true;
            }
            
            if (TryGetEnemy(identifier, out var enemy))
            {
                entity = enemy;
                return true;
            }

            entity = null;
            return false;
        }
    }
}