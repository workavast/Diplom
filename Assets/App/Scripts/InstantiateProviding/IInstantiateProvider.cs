using UnityEngine;

namespace App.InstantiateProviding
{
    public interface IInstantiateProvider
    {
        public T Instantiate<T>(T original) where T : Object;
    }
}