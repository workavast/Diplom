using System;
using App.Lobby.SelectedMission;
using App.Missions;
using App.Missions.UI;
using Fusion;
using UnityEngine;

namespace App.Lobby
{
    public class NetLobbyData : NetworkBehaviour, ISelectedMissionProvider
    {
        [SerializeField] private MissionSelector missionSelector;
        [SerializeField] private SelectedMissionView selectedMissionView;
        [SerializeField] private MissionsConfig missionsConfig;

        [Networked]
        [OnChangedRender(nameof(ActiveMissionIndexChanged))]
        [field: ReadOnly] public int ActiveMissionIndex { get; private set; } = 0;

        public Action<int> OnActiveMissionIndexChanged;

        public override void Spawned()
        {
            if (!HasStateAuthority)
                return;

            missionSelector.OnMissionClicked += SetMissionIndex;

            if (ActiveMissionIndex >= 0) 
                selectedMissionView.SetData(missionsConfig.Missions[ActiveMissionIndex]);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            missionSelector.OnMissionClicked -= SetMissionIndex;
        }
        
        public MissionConfig GetMission(int activeMissionIndex) 
            => missionsConfig.Missions[activeMissionIndex];

        private void SetMissionIndex(int index)
        {
            if (!HasStateAuthority)
            {
                Debug.LogError("You try set mission index when you doesnt have state authority");
                return;
            }

            if (index == ActiveMissionIndex)
            {
                Debug.LogWarning("You try set mission index that already active");
                return;
            }
            
            ActiveMissionIndex = index;
        }

        private void ActiveMissionIndexChanged()
        {
            selectedMissionView.SetData(missionsConfig.Missions[ActiveMissionIndex]);
            OnActiveMissionIndexChanged?.Invoke(ActiveMissionIndex);
        }
    }
}