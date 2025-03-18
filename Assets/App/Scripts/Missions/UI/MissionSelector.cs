using System;
using UnityEngine;

namespace App.Missions.UI
{
    public class MissionSelector : MonoBehaviour
    {
        [SerializeField] private MissionsConfig config;
        [SerializeField] private MissionBlock missionBlockPrefab;
        [SerializeField] private Transform missionsHolder;

        public event Action<int> OnMissionClicked;
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < config.Missions.Count; i++)
            {
                var missionBlock = Instantiate(missionBlockPrefab, missionsHolder);
                missionBlock.SetData(i, config.Missions[i]);
                missionBlock.OnClicked += (missionIndex) => OnMissionClicked?.Invoke(missionIndex);
            }
        }
    }
}