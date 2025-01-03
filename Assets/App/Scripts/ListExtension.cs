using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace App
{
    public static class ListExtension
    {
        public static int RandomIndex<T>(this List<T> list)
        {
            if (list == null)
                throw new NullReferenceException();
            if (list.Count <= 0)
                throw new IndexOutOfRangeException();
            
            var randomIndex = Random.Range(0, list.Count);
            return randomIndex;
        }
        
        public static T RandomValue<T>(this List<T> list)
        {
            var randomIndex = list.RandomIndex();
            return list[randomIndex];
        }
    }
}