using App.Players;
using TMPro;
using UnityEngine;

namespace App.UI.WeaponSelection
{
    public class SelectedWeaponView : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        
        private void Awake()
        {
            PlayerData.OnWeaponChanged += UpdateView;
            UpdateView();
        }

        private void OnDestroy()
        {
            PlayerData.OnWeaponChanged -= UpdateView;
        }
        
        private void UpdateView()
        {
            tmpText.text = PlayerData.SelectedWeapon.ToString();
        }
    }
}