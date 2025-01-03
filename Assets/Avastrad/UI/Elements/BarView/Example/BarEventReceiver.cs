using UnityEngine;

namespace Avastrad.UI.Elements.BarView.Example
{
    public class BarEventReceiver : MonoBehaviour
    {
        [SerializeField] private Bar bar;

        private void Awake()
        {
            bar.OnValueChanged.AddListener(InternalNotification);
        }

        [ContextMenu("_SetValue4")]
        private void SetValue4() 
            => bar.SetValue(0.4f);

        [ContextMenu("_SetValue5")]
        private void SetValue5() 
            => bar.SetValue(0.5f);
        
        [ContextMenu("_SetValue6")]
        private void SetValue6() 
            => bar.SetValue(0.6f);
        
        private void InternalNotification() 
            => Debug.Log($"CustomBarEventReceiver InternalNotification");

        public void _ExternalNotification() 
            => Debug.Log($"CustomBarEventReceiver ExternalNotification");
    }
}