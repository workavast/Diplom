using System.Collections.Generic;
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
        
        [Inject] private readonly PlayersEntitiesRepository _playersEntitiesRepository;
        
        private readonly List<LagCompensatedHit> _colliders = new(16);
        
        private PlayerRef Owner => ownerBehaviour.Object.InputAuthority;
        private NetworkRunner Runner => ownerBehaviour.Runner;
        private Vector3 Position => viewPivot.position;
        private float Radius => aiConfig.ViewRadius;
        private LayerMask PlayerLayers => aiConfig.PlayerLayers;
        
        public bool HasPlayerInZone()
        {
            foreach (var playerEntity in _playersEntitiesRepository.PlayerEntities)
            {
                var distance = Vector3.Distance(playerEntity.transform.position, viewPivot.position);
                if (Radius >= distance)
                    return true;
            }
            return false;
            
            
            
            const QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
            
            int collisions = Runner.LagCompensation.OverlapSphere(Position, Radius, Owner, _colliders, 
                PlayerLayers, HitOptions.None | HitOptions.IgnoreInputAuthority, true, triggerInteraction);

            for (int i = 0; i < collisions; i++)
            {
                var entity = _colliders[i].GameObject.GetComponent<IEntity>();
                if (!entity.IsAnyNull() && entity.EntityType == EntityType.Player)
                {
                    var direction = (entity.Transform.position.X0Z() - Position.X0Z()).normalized;
                    var distance = Vector3.Distance(entity.Transform.position, Position);

                    const HitOptions hitOptions = HitOptions.IncludePhysX;
                    var isHit = Runner.LagCompensation.Raycast(Position, direction, distance, Owner,
                        out var hit, -1, hitOptions, triggerInteraction);
                    Debug.Log($"Player in zone: [{isHit}] [{direction}] [{distance}]");
                    if (isHit)
                    {
                        var hitEntity = hit.GameObject.GetComponent<IEntity>();
                        Debug.Log(entity == hitEntity);
                        Debug.Log(entity.GameObject.name);
                        Debug.Log(hitEntity.GameObject.name);
                        if (entity == hitEntity)
                        {
                            Debug.Log("Player is visible");
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        public bool IsSeePlayer()
        {
            foreach (var playerEntity in _playersEntitiesRepository.PlayerEntities)
            {
                var distance = Vector3.Distance(playerEntity.transform.position, viewPivot.position);
                if (Radius >= distance)
                {
                    var direction = (playerEntity.transform.position.X0Z() - Position.X0Z()).normalized;

                    const HitOptions hitOptions = HitOptions.IncludePhysX;
                    const QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
                    var isHit = Runner.LagCompensation.Raycast(Position, direction, distance, Owner,
                        out var hit, -1, hitOptions, triggerInteraction);
                    if (isHit)
                    {
                        var hitEntity = hit.GameObject.GetComponent<IEntity>();
                        if (playerEntity.Is(hitEntity))
                            return true;
                    }                    
                }
            }
            return false;
        }
        
        public IEntity GetNearestPlayer()
        {
            return null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(viewPivot.position, Radius);
        }
    }
}