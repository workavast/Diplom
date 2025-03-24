using App.Weapons;
using UnityEngine;
using Zenject;

namespace App.UI.WeaponSelection
{
    public class WeaponSelectView : MonoBehaviour
    {
        [SerializeField] private WeaponSelectBtn weaponSelectBtnPrefab;
        [SerializeField] private Transform holder;
        
        [Inject] private readonly WeaponSelector _weaponSelector;

        private void Awake() 
            => Initialize();

        private void Initialize()
        {
            var weapons = _weaponSelector.GetAllWeapon();
            foreach (var weapon in weapons)
            {
                var view = Instantiate(weaponSelectBtnPrefab, holder);
                view.SetWeaponId(weapon);
                view.OnSelect += Select;
            }
        }
        
        private void Select(WeaponId weaponId) 
            => _weaponSelector.SelectWeapon(weaponId);
    }
}