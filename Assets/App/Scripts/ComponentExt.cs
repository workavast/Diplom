using System;
using UnityEngine;

namespace App
{
    public static class ComponentExt
    {
        public static T GetComponent<T>(this Component behaviour)
            where T : Component
        {
            var component = behaviour.GetComponent<T>();

            if (component == null) 
                component = behaviour.GetComponentInChildren<T>();

            if (component == null)
                throw new NullReferenceException();
            
            return component;
        }
    }
}