using TMPro;
using UnityEngine;

namespace App.Missions.UI
{
    public class SelectedMissionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameField;

        public void SetData(MissionConfig missionConfig)
        {
            nameField.text = missionConfig.MissionName;
        }
    }
}