using TMPro;
using UnityEngine;
using Zenject;

namespace App.Lobby.StartGameTimer
{
    public class StartGameTimerView : MonoBehaviour
    {
        [SerializeField] private string unActiveValue = "--:--";
        [SerializeField] private TMP_Text textField;
        
        [Inject] private IReadOnlyGameStartTimer _netStartGameTimer;

        private void LateUpdate()
        {
            if (_netStartGameTimer.IsActive)
                textField.text = GetTimeString();
            else
                textField.text = unActiveValue;
        }

        private string GetTimeString()
        {
            var time = _netStartGameTimer.GetTimeSpan();
            return $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}