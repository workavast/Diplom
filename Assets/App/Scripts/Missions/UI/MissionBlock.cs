using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Missions.UI
{
    public class MissionBlock : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameField;
        [SerializeField] private Button button;

        public int MissionIndex { get; private set; }

        public event Action<int> OnClicked;

        private void Awake()
        {
            button.onClick.AddListener(() => OnClicked?.Invoke(MissionIndex));
        }

        public void SetData(int missionIndex, MissionConfig missionConfig)
        {
            MissionIndex = missionIndex;

            nameField.text = missionConfig.MissionName;
        }
    }
}