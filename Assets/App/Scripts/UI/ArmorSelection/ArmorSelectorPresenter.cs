using System.Collections.Generic;
using App.Armor;
using App.Players;
using App.UI.Selection;
using Zenject;

namespace App.UI.ArmorSelection
{
    public class WeaponSelectorPresenter : Selector<int>
    {
        [Inject] private readonly ArmorsConfig _armorsConfig;

        protected override IReadOnlyList<int> GetIds()
        {
            var list = new List<int>(_armorsConfig.MaxArmorLevel);
            for (int i = 0; i < _armorsConfig.MaxArmorLevel; i++)
                list.Add(i);

            return list;
        }

        protected override string GetName(int id)
            => $"Level {id}";

        protected override void Select(int id) 
            => PlayerData.EquipArmor(id);
    }
}