using App.Ai.Config;
using App.Entities;
using App.Entities.Player;
using Avastrad.CheckOnNullLibrary;
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
        [SerializeField] private Transform distancePivot;
        
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        
        private PlayerRef Owner => ownerBehaviour.Object.InputAuthority;
        private NetworkRunner Runner => ownerBehaviour.Runner;
        private Vector3 ViewPosition => viewPivot.position;
        private Vector3 DistancePosition => distancePivot.position;
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
                var distance = Vector3.Distance(playerEntity.transform.position, ViewPosition);
                if (Radius >= distance)
                    return true;
            }
            return false;
        }

        public bool EntityIsVisible(IEntity entity)
        {
            if (entity.IsAnyNull())
                return false;

            if (entity.IsDead())
                return false;

            var viewDistance = Vector3.Distance(entity.Transform.position, ViewPosition);
            if (viewDistance > Radius) 
                return false;
            
            const HitOptions hitOptions = HitOptions.IncludePhysX;
            const QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
            var direction = (entity.Transform.position.X0Z() - DistancePosition.X0Z()).normalized;
            var eyeDistance = Vector3.Distance(entity.Transform.position, DistancePosition);
            var isHit = Runner.LagCompensation.Raycast(DistancePosition, direction, eyeDistance, Owner, out var hit, 
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
                    var distance = Vector3.Distance(playerEntity.transform.position, ViewPosition);
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
            Gizmos.DrawWireSphere(ViewPosition, Radius);
        }
    }
}