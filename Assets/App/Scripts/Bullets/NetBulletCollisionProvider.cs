using System;
using App.PlayerEntities;
using Fusion;
using UnityEngine;

namespace App.Bullets
{
    [RequireComponent(typeof(NetBulletController))]
    public class NetBulletCollisionProvider : NetworkBehaviour
    {
        [SerializeField] private LayerMask playerAndEnvironmentLayers;
        
        private NetBulletController _netBulletController;
        
        /// <summary>
        /// return hit point and normal of hit point
        /// </summary>
        public event Action<Vector3, Vector3> OnCollide;
        /// <summary>
        /// return player, hit point and normal of hit point
        /// </summary>
        public event Action<NetPlayerController, Vector3, Vector3> OnCollidePlayer; 

        private void Awake()
        {
            _netBulletController = GetComponent<NetBulletController>();
        }

        public override void FixedUpdateNetwork()
        {
            var isHit = Runner.LagCompensation.Raycast(transform.position, transform.forward, 
                _netBulletController.Speed * Runner.DeltaTime, Object.InputAuthority, out var hit, playerAndEnvironmentLayers);
            
            if (isHit)
            {
                var netPlayerController = hit.GameObject.GetComponent<NetPlayerController>();

                if (netPlayerController != null)
                    OnCollidePlayer?.Invoke(netPlayerController, hit.Point, hit.Normal);
                else
                    OnCollide?.Invoke(hit.Point, hit.Normal);
            }
        }
    }
}