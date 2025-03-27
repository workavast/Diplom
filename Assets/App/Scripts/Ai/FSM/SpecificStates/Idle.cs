using App.Entities;
using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.Ai.FSM
{
    public class Idle : AiState
    {
        public Idle(NetAi netAi, NetEntity netEntity) 
            : base(netAi, netEntity) { }

        protected override void OnEnterState()
        {
            Debug.Log("Idle");
        }

        protected override void OnFixedUpdate()
        {
            NetEntity.RotateByLookDirection(NetEntity.transform.forward.XZ());
            NetEntity.CalculateVelocity(0, 0, false);
        }
    }
}