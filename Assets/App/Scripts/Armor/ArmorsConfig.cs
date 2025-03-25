using System.Collections.Generic;
using UnityEngine;

namespace App.Armor
{
    [CreateAssetMenu(fileName = nameof(ArmorsConfig), menuName = Consts.AppName + "/Configs/Armor/" + nameof(ArmorsConfig))]
    public class ArmorsConfig : ScriptableObject
    {
        [SerializeField] private List<ArmorConfig> armorConfigs;

        public int MaxArmorLevel => armorConfigs.Capacity;
        
        public ArmorConfig GetArmor(int armorLevel)
        {
            if (armorLevel < 0)
            {
                Debug.LogError($"Armor level less then 0: [{armorLevel}]");
                armorLevel = 0;
            }

            if (armorLevel >= armorConfigs.Count)
            {
                Debug.LogError($"Armor level equal or higher then max armor level: [{armorLevel}] [{armorConfigs.Count}]");
                armorLevel = armorConfigs.Count - 1;
            }
            
            return armorConfigs[armorLevel];
        }
    }
}