using UnityEngine;

namespace App.InstantiateProviding
{
    public class InstantiateProvider : MonoBehaviour, IInstantiateProvider
    {
        private InstantiateProvider _instance;
            
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public new T Instantiate<T>(T original) where T : Object 
            => MonoBehaviour.Instantiate(original);
    }
}