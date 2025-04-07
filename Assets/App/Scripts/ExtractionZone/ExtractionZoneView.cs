using UnityEngine;

namespace App.ExtractionZone
{
    [RequireComponent(typeof(RectTransform))]
    public class ExtractionZoneView : MonoBehaviour
    {
        [SerializeField] private CountdownView countdownView;
        
        private RectTransform _rectTransform;
        
        private void Awake() 
            => _rectTransform = GetComponent<RectTransform>();
        
        public void ToggleCountdownVisibility(bool isVisible) 
            => countdownView.ToggleVisibility(isVisible);
        
        public void SetSize(float radius)
        {
            _rectTransform.sizeDelta = new Vector2(radius * 2, radius * 2);
        }
    }
}