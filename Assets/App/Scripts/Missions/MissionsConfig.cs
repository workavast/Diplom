using System.Collections.Generic;
using UnityEngine;

namespace App.Missions
{
    [CreateAssetMenu(fileName = nameof(MissionsConfig), menuName = Consts.AppName + "/Configs/Missions/" + nameof(MissionsConfig))]
    public class MissionsConfig : ScriptableObject
    {
        [SerializeField] private List<MissionConfig> missions;

        public IReadOnlyList<MissionConfig> Missions => missions;
    }
}