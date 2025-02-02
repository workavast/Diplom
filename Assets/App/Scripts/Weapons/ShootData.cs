using Fusion;
using UnityEngine;

namespace App.Weapons
{
    public struct ProjectileData : INetworkStruct
    {
        public Vector3 HitPosition;
        public Vector3 HitNormal;

        public ProjectileData(Vector3 hitPosition, Vector3 hitNormal)
        {
            HitPosition = hitPosition;
            HitNormal = hitNormal;
        }
    }
}