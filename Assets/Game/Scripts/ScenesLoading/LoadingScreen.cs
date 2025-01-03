using System;
using System.Collections;
using UnityEngine;

namespace BugStrategy.ScenesLoading
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeTime = 1;
        
        public bool IsShow { get; private set; }
        
        public event Action OnHided;

        public void Initialize() 
            => IsShow = gameObject.activeSelf;

        public void Show()
        {
            StopAllCoroutines();
            canvasGroup.alpha = 1;
            gameObject.SetActive(true);
            IsShow = true;
        }

        public void Hide(bool instantly)
        {
            if (instantly)
                HideInstantly();
            else
                HideWithFade();
        }
        
        private void HideWithFade()
        {
            StopAllCoroutines();
            StartCoroutine(Fade());
        }

        private void HideInstantly()
        {
            StopAllCoroutines();
            IsShow = false;
            gameObject.SetActive(false);
            OnHided?.Invoke();
        }

        private IEnumerator Fade()
        {
            float timer = 0;

            while (timer < fadeTime)
            {
                yield return new WaitForEndOfFrame();
                canvasGroup.alpha = fadeTime - timer;
                timer += Time.deltaTime;
            }
            
            IsShow = false;
            gameObject.SetActive(false);
            OnHided?.Invoke();
        }
    }
}