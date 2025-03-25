using System.Collections.Generic;
using UnityEngine;

namespace App.UI.Selection
{
    public abstract class Selector<TId> : MonoBehaviour
    {
        [SerializeField] private SelectionBtn<TId> weaponSelectBtnPrefab;
        [SerializeField] private Transform holder;
        
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

        protected abstract IReadOnlyList<TId> GetIds();

        protected abstract string GetName(TId id);

        protected abstract void Select(TId id);
    }
}