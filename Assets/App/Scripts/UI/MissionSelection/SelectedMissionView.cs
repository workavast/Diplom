using App.Missions;
using TMPro;
using UnityEngine;

namespace App.UI.MissionSelection
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