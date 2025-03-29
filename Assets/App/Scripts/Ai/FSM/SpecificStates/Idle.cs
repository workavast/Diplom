using App.Entities;
using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.Ai.FSM
{
    public class Idle : AiState
    {
        private readonly AiViewZone _aiViewZone;

        public Idle(NetAi netAi, NetEntity netEntity, AiViewZone aiViewZone)
            : base(netAi, netEntity)
        {
            _aiViewZone = aiViewZone;
        }

        protected override void OnEnterState()
        {
            Debug.Log("Idle");
        }

        protected override void OnFixedUpdate()
        {
            NetEntity.RotateByLookDirection(NetEntity.transform.forward.XZ());
            NetEntity.CalculateVelocity(0, 0, false);

            if (_aiViewZone.HasPlayerInZone())
            {
                Debug.Log("Player in zone");
                if (_aiViewZone.IsSeePlayer())
                {
                    Debug.Log("Player ii see");
                }
            }
        }
    }
}