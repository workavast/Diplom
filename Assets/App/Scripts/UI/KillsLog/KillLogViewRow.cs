using System;
using Avastrad.CustomTimer;
using TMPro;
using UnityEngine;

namespace App.UI.KillsLog
{
    public class KillLogViewRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private readonly Timer _timer = new(0);
        public event Action OnExistEnd;
        
        public void Initialize(float existTime)
        {
            _timer.SetMaxTime(existTime);
            _timer.SetPause();
            _timer.OnTimerEnd += () => OnExistEnd?.Invoke();
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
        }

        public void Show(string log)
        {
            gameObject.SetActive(true);

            text.text = log;
            _timer.Reset();
        }

        public void HideInstantly()
        {
            gameObject.SetActive(false);
        }
    }
}