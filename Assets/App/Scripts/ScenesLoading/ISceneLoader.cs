using System;

namespace BugStrategy.ScenesLoading
{
    public interface ISceneLoader
    {
        public event Action OnLoadingStarted;
        public event Action OnLoadingScreenHided;
        
        public void Initialize(bool endLoadingInstantly);
        public void LoadScene(int index);
    }
}