using TMPro;
using UnityEngine;

namespace App.ExtractionZone
{
    public class CountdownView : MonoBehaviour
    {
        [SerializeField] private NetExtractionZone netExtractionZone;
        [SerializeField] private TMP_Text tmpText;

        private void LateUpdate() 
            => tmpText.text = GetTime();

        public void ToggleVisibility(bool isVisible) 
            => gameObject.SetActive(isVisible);
        
        private string GetTime()
        {
            var time = netExtractionZone.GetTime();
            return $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}