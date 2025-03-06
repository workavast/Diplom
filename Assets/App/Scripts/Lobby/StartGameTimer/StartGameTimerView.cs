using TMPro;
using UnityEngine;

namespace App.Lobby
{
    public class StartGameTimerView : MonoBehaviour
    {
        [SerializeField] private NetStartGameTimer netStartGameTimer;
        [SerializeField] private TMP_Text textField;

        private void Awake()
        {
            netStartGameTimer.OnActivityChanged += ChangeState;
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
            var time = netStartGameTimer.GetTime();
            return $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}