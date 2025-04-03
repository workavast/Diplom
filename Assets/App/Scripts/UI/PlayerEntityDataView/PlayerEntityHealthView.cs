using App.Players;
using Avastrad.UI.Elements.BarView;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.UI.PlayerEntityDataView
{
    public class PlayerEntityHealthView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textView;
        [SerializeField] private Bar barView;
            
        [Inject] private readonly LocalPlayerProvider _playerProvider;

        private void LateUpdate()
        {
            if (_playerProvider.HasEntity)
            {
                barView.SetValue(_playerProvider.CurrentHealthPoints / _playerProvider.MaxHealthPoints);
                textView.text = $"{_playerProvider.CurrentHealthPoints:#0}/{_playerProvider.MaxHealthPoints:#0}";
            }
            else
            {
                barView.SetValue(0);
                textView.text = $"";
            }
        }
    }
}