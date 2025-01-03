using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace BlackRed.Game.NetworkRunning
{
    [DisallowMultipleComponent]
    public class NetworkObjectPoolDefault : NetworkObjectProviderDefault
    {
        [Tooltip("The objects to be pooled, leave it empty to pool every Network Object spawned")] [SerializeField]
        private List<NetworkObject> _poolableObjects;
        private DiContainer _diContainer;
        private Dictionary<NetworkObjectTypeId, Stack<NetworkObject>> _free = new();

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
        {
            NetworkObject instance = null;
            if (ShouldPool(prefab))
            {
                instance = GetObjectFromPool(prefab);
                instance.transform.position = Vector3.zero;
            }
            else
            {
                instance = Instantiate(prefab);
                _diContainer.InjectGameObject(instance.gameObject);
            }

            return instance;
        }

        protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
        {
            if (_free.TryGetValue(prefabId, out var stack))
            {
                instance.gameObject.SetActive(false);
                stack.Push(instance);
            }
            else
            {
                Destroy(instance.gameObject);
            }
        }

        private NetworkObject GetObjectFromPool(NetworkObject prefab)
        {
            NetworkObject instance = null;

            if (_free.TryGetValue(prefab.NetworkTypeId, out var stack))
            {
                while (stack.Count > 0 && instance == null) 
                    instance = stack.Pop();
            }

            if (instance == null)
                instance = GetNewInstance(prefab);

            instance.gameObject.SetActive(true);
            return instance;
        }

        private NetworkObject GetNewInstance(NetworkObject prefab)
        {
            var instance = Instantiate(prefab);
            _diContainer.InjectGameObject(instance.gameObject);

            if (!_free.TryGetValue(prefab.NetworkTypeId, out var stack))
            {
                stack = new Stack<NetworkObject>();
                _free.Add(prefab.NetworkTypeId, stack);
            }

            return instance;
        }

        private bool ShouldPool(NetworkObject prefab)
        {
            if (_poolableObjects.Count <= 0)
                return true;

            return IsPoolableObject(prefab);
        }

        private bool IsPoolableObject(NetworkObject networkObject)
        {
            foreach (var poolableObject in _poolableObjects)
                if (networkObject == poolableObject)
                    return true;

            return false;
        }
    }
}