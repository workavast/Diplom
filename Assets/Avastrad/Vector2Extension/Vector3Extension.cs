using UnityEngine;

namespace Avastrad.Vector2Extension
{
    public static class Vector3Extension
    {
        public static Vector3 X0Z(this Vector3 vector3, float o = 0) 
            => new(vector3.x, o, vector3.z);
        
        public static Vector2 XZ(this Vector3 vector3) 
            => new(vector3.x, vector3.z);
        public static Vector2 XY(this Vector3 vector3) 
            => new(vector3.x, vector3.y);

        public static Vector3 Div(this Vector3 vector3A, Vector3 vector3B)
        {
            var x = 0f;
            var y = 0f;
            var z = 0f;

            if (vector3B.x != 0) 
                x = vector3A.x / vector3B.x;

            if (vector3B.y != 0) 
                y = vector3A.y / vector3B.y;
            
            if (vector3B.z != 0) 
                z = vector3A.z / vector3B.z;
            
            return new Vector3(x, y, z);
        }
    }
}