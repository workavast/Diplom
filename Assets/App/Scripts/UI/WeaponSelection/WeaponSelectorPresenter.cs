using System.Collections.Generic;
using App.UI.Selection;
using App.Weapons;
using Zenject;

namespace App.UI.WeaponSelection
{
    public class WeaponSelectorPresenter : Selector<WeaponId>
    {
        [Inject] private readonly WeaponSelector _weaponSelector;

        protected override IReadOnlyList<WeaponId> GetIds() 
            => _weaponSelector.GetAllWeapon();

        protected override string GetName(WeaponId id) 
            => id.ToString();

        protected override void Select(WeaponId id)
            => _weaponSelector.SelectWeapon(id);
    }
}