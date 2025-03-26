using System.Collections.Generic;
using App.UI.WindowsSwitching;
using UnityEngine;

namespace App.UI.Selection
{
    public abstract class Selector<TId> : MonoBehaviour, IWindow
    {
        [SerializeField] private SelectionBtn<TId> weaponSelectBtnPrefab;
        [SerializeField] private Transform holder;
        [field: SerializeField] public string Id { get; private set; }
        
        private void Awake() 
            => Initialize();

        private void Initialize()
        {
            var allIds = GetIds();
            foreach (var id in allIds)
            {
                var view = Instantiate(weaponSelectBtnPrefab, holder);
                view.SetData(id, GetName(id));
                view.OnClick += Select;
            }
        }
        
        public virtual void Toggle(bool isVisible) 
            => gameObject.SetActive(isVisible);

        protected abstract IReadOnlyList<TId> GetIds();

        protected abstract string GetName(TId id);

        protected abstract void Select(TId id);
    }
}