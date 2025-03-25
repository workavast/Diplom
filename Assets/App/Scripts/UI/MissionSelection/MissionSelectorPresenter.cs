using System;
using System.Collections.Generic;
using App.Missions;
using App.UI.Selection;
using UnityEngine;

namespace App.UI.MissionSelection
{
    public class MissionSelectorPresenter : Selector<int>
    {
        [SerializeField] private MissionsConfig config;

        public event Action<int> OnMissionClicked;
        
        protected override IReadOnlyList<int> GetIds()
        {
            var allIds = new List<int>(config.Missions.Count);
            for (int i = 0; i < config.Missions.Count; i++)
                allIds.Add(i);

            return allIds;
        }

        protected override string GetName(int id) 
            => config.Missions[id].MissionName;

        protected override void Select(int id) 
            => OnMissionClicked?.Invoke(id);
    }
}