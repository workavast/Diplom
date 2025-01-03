using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    public class ScreenBase : MonoBehaviour
    {
        public virtual void Initialize() {}
        
        public virtual void Show()
            => gameObject.SetActive(true);

        public virtual void Hide()
            => HideInstantly();
        
        public virtual void HideInstantly()
            => gameObject.SetActive(false);
    }
}