using System;
using UnityEngine;
using UnityEngine.Events;

namespace Avastrad.UI.Elements.BarView
{
    [ExecuteAlways]
    public class Bar : MonoBehaviour
    {
        //this fields used in editor OnInspectorGUI, dont change them signatures or names
        [SerializeField] private RectTransform fill;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue = 1;
        [SerializeField] private bool wholeNumbers;
        [SerializeField] private float value;
        [SerializeField] private UnityEvent onValueChanged;

        public RectTransform FillRect => fill;
        public float MinValue => minValue;
        public float MaxValue => maxValue;
        public float Value { get; set; }
        public UnityEvent OnValueChanged => onValueChanged;
        public float FillPercentage
        {
            get
            {
                if (maxValue - minValue == 0)
                    return 0;
                
                return Mathf.Abs((Value - minValue) / (maxValue - minValue));
            }
        }

        private DrivenRectTransformTracker _tracker;

        private void Awake()
        {
            SetFill(fill);
            SetValueWithoutNotification(value);
        }
        
        public void SetValueWithoutNotification(float newValue)
        {
            newValue = Mathf.Clamp(newValue, minValue, maxValue);
            if (wholeNumbers)
                newValue = (float)Math.Round(newValue, MidpointRounding.AwayFromZero);
            Value = value = newValue;
            UpdateFillAnchor();
        }
        
        public void SetValue(float newValue)
        {
            SetValueWithoutNotification(newValue);
            
            onValueChanged.Invoke();
        }

        //This method invoked by using reflection, dont change it signature or name
        private void SetFill(RectTransform newFill)
        {
            _tracker.Clear();
            if (newFill != null)
            {
                _tracker.Add(this, newFill, DrivenTransformProperties.Anchors);
                UpdateFillAnchor();
            }
        }

        private void UpdateFillAnchor()
        {
            if (fill == null)
                return;
            
            var currentAnchorMax = fill.anchorMax;
            currentAnchorMax.x = FillPercentage;
            currentAnchorMax.y = 1;
            fill.anchorMax = currentAnchorMax;
        }
        
        private void OnDestroy()
        {
            _tracker.Clear();
        }
    }
}