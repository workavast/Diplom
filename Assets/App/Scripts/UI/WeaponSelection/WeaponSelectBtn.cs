using System;
using App.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.WeaponSelection
{
    public class WeaponSelectBtn : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        
        private Button _button;
        private WeaponId _weaponId;
        
        public event Action<WeaponId> OnSelect;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnSelect?.Invoke(_weaponId));
        }

        public void SetWeaponId(WeaponId weaponId)
        {
            _weaponId = weaponId;
            tmpText.text = _weaponId.ToString();
        }
    }
}