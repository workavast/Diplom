using System.Collections.Generic;
using App.Entities;
using Avastrad.CheckOnNullLibrary;
using Avastrad.Vector2Extension;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Ai.FSM
{
    public abstract class AiState : State
    {
        protected readonly NetAi NetAi;
        protected readonly NetEntity NetEntity;

        protected PlayerRef Owner => NetAi.Object.InputAuthority;
        protected AiConfig AiConfig => NetAi.AiConfig;
        protected NetworkRunner Runner => NetAi.Runner;
        
        protected AiState(NetAi netAi, NetEntity netEntity)
        {
            NetAi = netAi;
            NetEntity = netEntity;
        }

        protected bool HasPlayerInZone(Vector3 position, float radius, List<LagCompensatedHit> colliders, LayerMask layerMask)
        {
            const QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
            
            int collisions = Runner.LagCompensation.OverlapSphere(position, radius, Owner, colliders, 
                layerMask, HitOptions.None | HitOptions.IgnoreInputAuthority, true, triggerInteraction);

            for (int i = 0; i < collisions; i++)
            {
                var entity = colliders[i].GameObject.GetComponent<IEntity>();
                if (!entity.IsAnyNull() && entity.EntityType == EntityType.Player)
                {
                    var direction = (entity.Transform.position.X0Z() - position.X0Z()).normalized;
                    var distance = Vector3.Distance(entity.Transform.position, position);

                    const HitOptions hitOptions = HitOptions.IncludePhysX;
                    var isHit = Runner.LagCompensation.Raycast(position, direction, distance, Owner,
                        out var hit, -1, hitOptions, triggerInteraction);
                    Debug.Log($"[{isHit}] [{direction}] [{distance}]");
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
    }
}