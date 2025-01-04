using System.Collections.Generic;
using App.DiProviding;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.NetworkRunning
{
    [DisallowMultipleComponent]
    public class NetworkObjectPoolDefault : NetworkObjectProviderDefault
    {
        [Tooltip("The objects to be pooled, leave it empty to pool every Network Object spawned")] [SerializeField]
        private List<NetworkObject> poolableObjects;
        private IDiProvider _diProvider;
        private readonly Dictionary<NetworkObjectTypeId, Stack<NetworkObject>> _free = new();

        [Inject]
        public void Construct(IDiProvider diContainer)
        {
            _diProvider = diContainer;
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
                _diProvider.DiContainer.InjectGameObject(instance.gameObject);
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
            _diProvider.DiContainer.InjectGameObject(instance.gameObject);

            if (!_free.TryGetValue(prefab.NetworkTypeId, out var stack))
            {
                stack = new Stack<NetworkObject>();
                _free.Add(prefab.NetworkTypeId, stack);
            }

            return instance;
        }

        private bool ShouldPool(NetworkObject prefab)
        {
            if (poolableObjects.Count <= 0)
                return true;

            return IsPoolableObject(prefab);
        }

        private bool IsPoolableObject(NetworkObject networkObject)
        {
            foreach (var poolableObject in poolableObjects)
                if (networkObject == poolableObject)
                    return true;

            return false;
        }
    }
}