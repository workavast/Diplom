using UnityEngine;

namespace BlackRed.Game.InstantiateProviding
{
    public interface IInstantiateProvider
    {
        public T Instantiate<T>(T original) where T : Object;
    }
}