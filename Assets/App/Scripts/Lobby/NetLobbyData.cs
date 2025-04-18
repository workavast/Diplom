using System;
using App.Lobby.SelectedMission;
using App.Missions;
using App.UI.MissionSelection;
using Fusion;
using UnityEngine;

namespace App.Lobby
{
    public class NetLobbyData : NetworkBehaviour, ISelectedMissionProvider
    {
        [SerializeField] private MissionSelectorPresenter missionSelectorPresenter;
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

            missionSelectorPresenter.OnMissionClicked += SetMissionIndex;

            if (ActiveMissionIndex >= 0)
            {
                selectedMissionView.SetData(missionsConfig.Missions[ActiveMissionIndex]);
                missionSelectorPresenter.SetActiveMarker(missionsConfig.Missions[ActiveMissionIndex]);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            missionSelectorPresenter.OnMissionClicked -= SetMissionIndex;
        }
        
        public MissionConfig GetMission(int activeMissionIndex) 
            => missionsConfig.Missions[activeMissionIndex];

        private void SetMissionIndex(MissionConfig missionConfig)
        {
            if (!HasStateAuthority)
            {
                Debug.LogError("You try set mission index when you doesnt have state authority");
                return;
            }

            var missionIndex = missionsConfig.GetIndex(missionConfig);
            if (missionIndex == ActiveMissionIndex)
            {
                Debug.LogWarning("You try set mission index that already active");
                return;
            }
            
            ActiveMissionIndex = missionIndex;
        }

        private void ActiveMissionIndexChanged()
        {
            selectedMissionView.SetData(missionsConfig.Missions[ActiveMissionIndex]);
            if (!HasStateAuthority)
                missionSelectorPresenter.SetActiveMarker(missionsConfig.Missions[ActiveMissionIndex]);

            OnActiveMissionIndexChanged?.Invoke(ActiveMissionIndex);
        }
    }
}