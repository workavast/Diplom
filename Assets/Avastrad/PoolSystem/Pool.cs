using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Avastrad.PoolSystem
{
    public class Pool<TElement> where TElement : IPoolable<TElement>
    {
        private readonly Func<TElement> _instantiateDelegate;
        private readonly Queue<TElement> _freeElements;
        private readonly List<TElement> _busyElements;
        private readonly bool _expandable;
        private readonly bool _reExtracted;
        private readonly int _capacity;

        public IReadOnlyList<TElement> FreeElements => _freeElements.ToArray();
        public IReadOnlyList<TElement> BusyElements => _busyElements;

        public Pool(Func<TElement> instantiateDelegate, bool expandable = true, bool reExtracted = false,
            int initialElementCount = 0, int capacity = 0)
        {
            if (instantiateDelegate == null) throw new Exception("instantiateDelegate is null");
            if (initialElementCount < 0) throw new Exception("initialElementCount can't be less then 0");
            if (!expandable && capacity <= 0) throw new Exception("In not expandable pool capacity can't be less or equal to 0");
            if (!expandable && initialElementCount > capacity) throw new Exception("In not expandable pool initialElementCount can't be bigger then capacity");

            _instantiateDelegate = instantiateDelegate;
            _freeElements = new Queue<TElement>();
            _busyElements = new List<TElement>();
            _expandable = expandable;
            _reExtracted = reExtracted;
            _capacity = capacity;

            for (int i = 0; i < initialElementCount; i++) InstantiateElement();
        }

        public bool ExtractElement(out TElement extractedElement)
        {
            extractedElement = default;

            if (_freeElements.Count == 0)
            {
                if (_expandable || _busyElements.Count < _capacity)
                {
                    InstantiateElement();
                }
                else
                {
                    if (_reExtracted) ReturnElement(_busyElements.First());
                    else return false;
                }
            }

            extractedElement = _freeElements.Dequeue();
            _busyElements.Add(extractedElement);

            extractedElement.DestroyElementEvent += OnDestroyElement;
            extractedElement.ReturnElementEvent += OnReturnElement;
            extractedElement.OnElementExtractFromPool();

            return true;
        }

        private void InstantiateElement()
        {
            _freeElements.Enqueue(_instantiateDelegate());
        }

        private void OnDestroyElement(TElement element)
        {
            if (_busyElements.Remove(element))
            {
                element.DestroyElementEvent -= OnDestroyElement;
                element.ReturnElementEvent -= OnReturnElement;
            }
        }

        private void OnReturnElement(TElement element)
        {
            if (_busyElements.Contains(element))
            {
                _freeElements.Enqueue(element);

                element.DestroyElementEvent -= OnDestroyElement;
                element.ReturnElementEvent -= OnReturnElement;

                _busyElements.Remove(element);

                element.OnElementReturnInPool();
            }
        }

        public void ReturnElement(TElement element) 
            => OnReturnElement(element);
    }

    public class Pool<TElement, TId> where TElement : IPoolable<TElement, TId>
    {
        private readonly Func<TId, TElement> _instantiateDelegate;
        private readonly Dictionary<TId, Queue<TElement>> _freeElements;
        private readonly Dictionary<TId, List<TElement>> _busyElements;
        private readonly bool _expandableId;
        private readonly bool _expandableElement;
        private readonly bool _reExtracted;
        private readonly int _capacityIds;
        private readonly Dictionary<TId, int> _capacityElements;

        public IReadOnlyDictionary<TId, IReadOnlyList<TElement>> FreeElements =>
            _freeElements.ToDictionary(x => x.Key, x => x.Value.ToList() as IReadOnlyList<TElement>);

        public IReadOnlyDictionary<TId, IReadOnlyList<TElement>> BusyElements => 
            _busyElements.ToDictionary(x => x.Key, x=> x.Value as IReadOnlyList<TElement>);

        public IReadOnlyList<IReadOnlyList<TElement>> BusyElementsValues => _busyElements.Values.ToList();

        public Pool(
            Func<TId, TElement> instantiateDelegate,
            bool expandableId = true,
            bool expandableElement = true,
            bool reExtracted = false, 
            int capacityIds = 0, 
            ReadOnlyDictionary<TId, int> capacityElements = null,
            ReadOnlyDictionary<TId, int> initialElementsCounts = null)
        {
            if (instantiateDelegate == null) throw new Exception("instantiateDelegate is null");

            if (!expandableId)
            {
                if (capacityIds <= 0) throw new Exception("In not expandableId pool maxSizeIds can't be less or equal to 0");
                if (capacityElements == null) throw new Exception("In not expandableId pool maxSizes can't be null");
                if (capacityElements.Count <= 0) throw new Exception("In not expandableId pool maxSizes.Count can't be 0");
                if (capacityElements.Count > capacityIds) throw new Exception("In not expandableId pool maxSizes.Count can't be bigger than maxSizeIds");
            }

            if (!expandableElement)
            {
                if (capacityElements == null) throw new Exception("In not expandableElement pool maxSizes can't be null");
                if (capacityElements.Count <= 0) throw new Exception("In not expandableElement pool maxSizes.Count can't be 0");

                foreach (var size in capacityElements)
                    if (size.Value <= 0) throw new Exception($"In not expandableElement pool maxSize of id {size.Key} can't be less or equal to 0");
            }

            if (!expandableId && !expandableElement && capacityIds != capacityElements.Count)
                throw new Exception("In not expandableId and expandableElement pool maxSizes of id can't be not equal maxSizeIds");

            _instantiateDelegate = instantiateDelegate;
            _freeElements = new Dictionary<TId, Queue<TElement>>();
            _busyElements = new Dictionary<TId, List<TElement>>();
            _expandableId = expandableId;
            _expandableElement = expandableElement;
            _reExtracted = reExtracted;
            _capacityIds = capacityIds;
            _capacityElements = expandableElement ? new Dictionary<TId, int>() : capacityElements.ToDictionary(element => element.Key, element => element.Value);

            if (initialElementsCounts != null)
            {
                foreach (var element in initialElementsCounts)
                {
                    if (element.Value > 0 && (expandableId || _freeElements.Count < capacityIds))
                        for (int i = 0; i < element.Value; i++)
                        {
                            if (_expandableId && _expandableElement ||
                                (!_expandableId && _expandableElement && _freeElements.Count < _capacityIds) ||
                                (_capacityElements.ContainsKey(element.Key) && (!_freeElements.ContainsKey(element.Key) || 
                                    _freeElements[element.Key].Count < _capacityElements[element.Key])))
                                InstantiateElement(element.Key);
                            else break;
                        }
                }
            }
        }

        public bool ExtractElement(TId id, out TElement extractedElement)
        {
            if (!_expandableId && _freeElements.Count >= _capacityIds && !_freeElements.ContainsKey(id))
                throw new Exception("In not expandableId pool _freeElements dont have this id");

            extractedElement = default;

            if (!_freeElements.ContainsKey(id))
            {
                if (_expandableId && _expandableElement ||
                    (!_expandableId && _expandableElement && _freeElements.Count < _capacityIds) ||
                    (_capacityElements.ContainsKey(id) && (!_freeElements.ContainsKey(id) || _freeElements[id].Count < _capacityElements[id])))
                    InstantiateElement(id);
                else return false;
            }
            else
            {
                if (_freeElements[id].Count == 0)
                {
                    if (_expandableElement || (_busyElements.ContainsKey(id) && _busyElements[id].Count < _capacityElements[id]))
                    {
                        InstantiateElement(id);
                    }
                    else
                    {
                        if (_reExtracted && _busyElements.ContainsKey(id) && _busyElements[id].Count > 0)
                            ReturnElement(_busyElements[id].First());
                        else return false;
                    }
                }
            }

            extractedElement = _freeElements[id].Dequeue();
            _busyElements[id].Add(extractedElement);

            extractedElement.DestroyElementEvent += OnDestroyElement;
            extractedElement.ReturnElementEvent += OnReturnElement;
            extractedElement.OnElementExtractFromPool();

            return true;
        }

        private void InstantiateElement(TId id)
        {
            if (_freeElements.ContainsKey(id))
            {
                if (_expandableElement || _busyElements[id].Count < _capacityElements[id])
                    _freeElements[id].Enqueue(_instantiateDelegate(id));
                else 
                    throw new Exception("_busyElements[id].Count >= _capacityElements[id]");
            }
            else
            {
                if (_expandableId || _freeElements.Count < _capacityIds)
                {
                    _freeElements.Add(id, new Queue<TElement>());
                    if (!_busyElements.ContainsKey(id)) 
                        _busyElements.Add(id, new List<TElement>());

                    _freeElements[id].Enqueue(_instantiateDelegate(id));
                }
                else
                {
                    throw new Exception("in no expandable poolId _freeElements.Count >= _capacityIds");
                }
            }
        }

        private void OnDestroyElement(TElement element)
        {
            if (!_busyElements.ContainsKey(element.PoolId)) return;

            if (_busyElements[element.PoolId].Remove(element))
            {
                element.DestroyElementEvent -= OnDestroyElement;
                element.ReturnElementEvent -= OnReturnElement;
            }
        }

        private void OnReturnElement(TElement element)
        {
            if (!_busyElements.ContainsKey(element.PoolId))
            {
                Debug.LogWarning("You try return pool element that id don't contained in _busyElements");
                return;
            }

            if (_busyElements[element.PoolId].Contains(element))
            {
                _freeElements[element.PoolId].Enqueue(element);

                element.DestroyElementEvent -= OnDestroyElement;
                element.ReturnElementEvent -= OnReturnElement;

                _busyElements[element.PoolId].Remove(element);

                element.OnElementReturnInPool();
            }
            else
            {
                Debug.LogWarning($"You try return pool element that don't contained in _busyElements[{element.PoolId}]");
            }
        }

        public void ReturnElement(TElement element) 
            => OnReturnElement(element);
    }
}