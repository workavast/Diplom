using UnityEngine;

namespace Avastrad.Vector2Extension
{
    public static class Vector3Extension
    {
        public static Vector2 XZ(this Vector3 vector2) 
            => new(vector2.x, vector2.z);
    }
}