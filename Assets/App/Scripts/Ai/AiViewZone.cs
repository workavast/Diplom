using App.Entities;
using App.Entities.Player;
using Avastrad.Vector2Extension;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.Ai
{
    public class AiViewZone : MonoBehaviour
    {
        [SerializeField] private NetworkBehaviour ownerBehaviour;
        [SerializeField] private AiConfig aiConfig;
        [SerializeField] private Transform viewPivot;
        
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        
        private PlayerRef Owner => ownerBehaviour.Object.InputAuthority;
        private NetworkRunner Runner => ownerBehaviour.Runner;
        private Vector3 Position => viewPivot.position;
        private float Radius => aiConfig.ViewRadius;
        private LayerMask PlayerLayers => aiConfig.PlayerLayers;
        
        public bool IsSeeAnyPlayer()
        {
            if (!HasPlayerInZone())
                return false;

            foreach (var playerEntity in _playersEntitiesRepository.PlayerEntities)
                if (EntityIsVisible(playerEntity))
                    return true;
            
            return false;
        }
        
        public bool HasPlayerInZone()
        {
            foreach (var playerEntity in _playersEntitiesRepository.PlayerEntities)
            {
                var distance = Vector3.Distance(playerEntity.transform.position, viewPivot.position);
                if (Radius >= distance)
                    return true;
            }
            return false;
        }

        public bool EntityIsVisible(IEntity entity)
        {
            if (entity.IsDead())
                return false;

            var distance = Vector3.Distance(entity.Transform.position, viewPivot.position);
            if (!(Radius >= distance)) 
                return false;
            
            const HitOptions hitOptions = HitOptions.IncludePhysX;
            const QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
            var direction = (entity.Transform.position.X0Z() - Position.X0Z()).normalized;
            var isHit = Runner.LagCompensation.Raycast(Position, direction, distance, Owner, out var hit, 
                -1, hitOptions, triggerInteraction);
            if (isHit)
            {
                var hitEntity = hit.GameObject.GetComponent<IEntity>();
                if (entity.Is(hitEntity))
                    return true;
            }

            return false;
        }
        
        public IEntity GetNearestVisiblePlayer()
        {
            IEntity nearest = null;
            float minDistance = float.MaxValue;

            foreach (var playerEntity in _playersEntitiesRepository.PlayerEntities)
            {
                if (playerEntity.IsAlive() && EntityIsVisible(playerEntity))
                {
                    var distance = Vector3.Distance(playerEntity.transform.position, Position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = playerEntity;
                    }
                }
            }

            return nearest;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(viewPivot.position, Radius);
        }
    }
}