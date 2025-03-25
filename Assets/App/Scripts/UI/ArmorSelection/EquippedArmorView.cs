using App.Players;
using TMPro;
using UnityEngine;

namespace App.UI.ArmorSelection
{
    public class EquippedArmorView : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        
        private void Awake()
        {
            PlayerData.OnArmorLevelChanged += UpdateView;
            UpdateView();
        }

        private void OnDestroy() 
            => PlayerData.OnArmorLevelChanged -= UpdateView;

        private void UpdateView() 
            => tmpText.text = $"Armor Level {PlayerData.EquippedArmorLevel}";
    }
}