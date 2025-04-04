using System;

namespace Avastrad.ScenesLoading
{
    public interface ILoadingScreen
    {
        public bool IsVisible { get; }

        public event Action OnHided;
        
        public void Show(bool instantly, Action onShowedCallback);
        public void Hide(bool instantly);
    }
}