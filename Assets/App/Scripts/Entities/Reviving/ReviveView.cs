using UnityEngine;
using UnityEngine.UI;

namespace App.Entities.Reviving
{
    public class ReviveView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Canvas canvas;
        
        public void ToggleVisibility(bool isVisible)
        {
            canvas.enabled = isVisible;
        }

        /// <param name="percentage"> value [0,1] </param>
        public void SetValue(float percentage)
        {
            image.fillAmount = percentage;
        }
    }
}