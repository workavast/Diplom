using App.Players;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.UI.PlayerEntityDataView
{
    public class PlayerEntityAmmoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textView;
            
        [Inject] private readonly LocalPlayerProvider _playerProvider;

        private void LateUpdate()
        {
            if (_playerProvider.HasEntity)
                textView.text = $"{_playerProvider.CurrentAmmo:#0}/{_playerProvider.MaxAmmo:#0}";
            else
                textView.text = $"";
        }
    }
}