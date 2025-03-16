using System;
using Fusion;
using UnityEngine;

namespace App.Lobby
{
    public class NetLobbyData : NetworkBehaviour
    {
        [Networked] [OnChangedRender(nameof(ActiveMissionIndexChanged))] [field: ReadOnly]
        public int ActiveMissionIndex { get; private set; }

        public Action<int> OnActiveMissionIndexChanged;
        
        public void SetMissionIndex(int index)
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
            OnActiveMissionIndexChanged?.Invoke(ActiveMissionIndex);
        }
    }
}