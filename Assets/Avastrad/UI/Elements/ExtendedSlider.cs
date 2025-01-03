using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Avastrad.UI.Elements
{
    public class ExtendedSlider : Slider
    {
        public Action OnPointerDownEvent;
        public Action OnPointerUpEvent;

        public Action OnPointerEnterEvent;
        public Action OnPointerExitEvent;
        
        public Action OnDragEvent;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnPointerDownEvent?.Invoke();
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnPointerUpEvent?.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnPointerEnterEvent?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            OnPointerExitEvent?.Invoke();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            OnDragEvent?.Invoke();
        }
    }
}