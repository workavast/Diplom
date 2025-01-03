using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App
{
    public static class NullCheck
    {
        public static bool IsNull<T>(this T t) => t == null;

        public static T IsNullGetInChildren<T>(this T t, MonoBehaviour parent)
            where T : Object
        {
            if (t == null)
            {
                Debug.LogWarning("t is null");

                t = parent.GetComponentInChildren<T>(true);
                if (t == null)
                    throw new NullReferenceException("cant find t in children");
            }
            
            return t;
        }
        
        public static T IsNullFindOfType<T>(this T t)
            where T : Object
        {
            if (t == null)
            {
                Debug.LogWarning("t is null");

                t = Object.FindObjectOfType<T>(true);
                if (t == null)
                    throw new NullReferenceException("cant find t on the scene");
            }

            return t;
        }
    }
}