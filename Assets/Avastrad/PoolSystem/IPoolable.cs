using System;

namespace Avastrad.PoolSystem
{
    public interface IPoolable<TElement>
    {
        public event Action<TElement> ReturnElementEvent;
        public event Action<TElement> DestroyElementEvent;
    
        public void OnElementExtractFromPool();
        public void OnElementReturnInPool();
    }

    public interface IPoolable<TElement, TId> : IPoolable<TElement>
    {
        public TId PoolId { get;}
    }
}