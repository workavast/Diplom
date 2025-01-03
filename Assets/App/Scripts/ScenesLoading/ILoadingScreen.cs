using System;

namespace BugStrategy.ScenesLoading
{
    public interface ILoadingScreen
    {
        public bool IsShow { get; }

        public event Action OnHided;
        
        public void Show();
        public void Hide(bool endInstantly);
    }
}