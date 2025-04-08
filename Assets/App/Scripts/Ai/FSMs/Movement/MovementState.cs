using App.Entities;
using Avastrad.Vector2Extension;
using Fusion.Addons.FSM;
using UnityEngine;

namespace App.Ai.FSMs.Movement
{
    public class MovementState : State<MovementState>
    {
        protected readonly NetEntity Entity;

        protected MovementState(NetEntity entity)
        {
            Entity = entity;
        }
        
        protected bool ArrivePosition(Vector3 targetPosition, float tolerance) 
            => Vector2.Distance(Entity.transform.position.XZ(), targetPosition.XZ()) <= tolerance;
    }
}