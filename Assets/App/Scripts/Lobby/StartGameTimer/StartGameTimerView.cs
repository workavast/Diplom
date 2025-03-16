using TMPro;
using UnityEngine;
using Zenject;

namespace App.Lobby.StartGameTimer
{
    public class StartGameTimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        
        [Inject] private IReadOnlyGameStartTimer _netStartGameTimer;

        private void Awake()
        {
            _netStartGameTimer.OnActivityChanged += ChangeState;
            ChangeState(_netStartGameTimer.IsActive);
        }

        private void LateUpdate()
        {
            textField.text = GetTimeString();
        }

        private void ChangeState(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        private string GetTimeString()
        {
            var time = _netStartGameTimer.GetTimeSpan();
            return $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}